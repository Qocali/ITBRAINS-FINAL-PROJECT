
using Final.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }
       public DbSet<Bio> bios { get; set; }
       public DbSet<Slider> sliders { get; set; }
       public DbSet<Teacher> Teacher { get; set; }
       public DbSet<TeacherDetails> TeacherDetails { get; set; }
       public DbSet<TeacherSkills> TeacherSkills { get; set; }
      
       public DbSet<Board> Boards { get; set; }
       public DbSet<Video> Videos { get; set; }
       public DbSet<Events> Events { get; set; }
       public DbSet<EventSpeaker> EventSpeakers { get; set; }
       public DbSet<Speaker> Speakers { get; set; }
       public DbSet<Course> Courses { get; set; }
       public DbSet<CourseDetail> CourseDetails { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<CourseCategories> CourseCategories { get; set; }
        public DbSet<TeacherSkills> TeacherSkillsmanies { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogDetail> BlogDetails { get; set; }
       
        public DbSet<Subscribe> subscribes { get; set; }
        public DbSet<About> Abouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bio>().HasData(
                new Bio { Id = 3,Logo = "footer-logo", Facebook = "https://www.facebook.com/qocheli.babayev", Twitter = "https://www.linkedin.com/in/qocheli-babayev-141009218",Pinterest = "https://www.Pinterest.com/in/qocheli-babayev-141009218",Vimeo= "https://www.Vimeo.com/qocheli.babayev" }
                );
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<About>().HasData(
                new About { Id = 3,Title= "<h2>bizim haqqimizda melumat</h2>",SubTitle= "<h3>drtfgyhuj</h3>" ,Description= "rtyuiof",Image= "about.png"}
                );

        }
    }
}
