using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using EmotionCore.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;


namespace EmotionCore.Util
{
    public class FaceHelper
    {
        private readonly IFaceClient faceClient;

        public FaceHelper(string apiKey, string baseUri)
        {

            faceClient = new FaceClient(
                new ApiKeyServiceClientCredentials(apiKey),
                new DelegatingHandler[] {}
            );

            faceClient.BaseUri = new Uri(baseUri);
 
        }

        public async Task<EmoPicture>DetectedFacesAndExtracFacesAsync(Stream imageStream, IList<FaceAttributeType> faceAttributes)
        {

            IList<DetectedFace> faceList = await faceClient.Face.DetectWithStreamAsync(imageStream, true, false, faceAttributes);

            EmoPicture emoPicture = new EmoPicture();
            emoPicture.Faces = ExtractFaces(faceList, emoPicture);

            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtractFaces(IList<DetectedFace> faceList, EmoPicture emoPicture)
        {

            var listaFaces = new ObservableCollection<EmoFace>();

            foreach (var face in faceList)
            {

                var emoFace = new EmoFace()
                {
                    X = face.FaceRectangle.Left,
                    Y = face.FaceRectangle.Top,
                    Width = face.FaceRectangle.Width,
                    Height = face.FaceRectangle.Height,
                    Picture = emoPicture
                };

              

                emoFace.Emotions = ProccessEmotions(face.FaceAttributes.Emotion , emoFace);
                listaFaces.Add(emoFace);
                
            }

            return listaFaces;
        }

        private ObservableCollection<EmoEmotion> ProccessEmotions(Emotion scores, EmoFace emoFace)
        {
            var emotionList = new ObservableCollection<EmoEmotion>();
            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

          
            var filterProperties = properties.Where(p => p.PropertyType == typeof(double));

            var emotype = EmoEmotionEnum.Undetermined;

            foreach (var prop in filterProperties)
            {
                if (!Enum.TryParse<EmoEmotionEnum>(prop.Name , out emotype))
                    emotype = EmoEmotionEnum.Undetermined;

                var emoEmotion = new EmoEmotion()
                {
                    Score = Convert.ToSingle( prop.GetValue(scores)),
                    EmotionType = emotype,
                    Face = emoFace 
                 };
               
                emotionList.Add(emoEmotion);

            }

            return emotionList; 

        }
    }
}
