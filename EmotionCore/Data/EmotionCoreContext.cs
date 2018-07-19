using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmotionCore.Models
{
    public class EmotionCoreContext : DbContext
    {
        public EmotionCoreContext (DbContextOptions<EmotionCoreContext> options)
            : base(options)
        {
       
        
        }
        

        public DbSet<EmoPicture> EmoPictures { get; set; }
        public DbSet<EmoFace> EmoFaces { get; set; }
        public DbSet<EmoEmotion> EmoEmotions { get; set; }
    }
}
