using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmotionCore.Models
{
    public class EmoPicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public string Path { get; set; }
        public virtual ObservableCollection<EmoFace> Faces { get; set; }
    }
}
