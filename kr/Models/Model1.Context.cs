//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kr.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntities : DbContext
    {
        public dbEntities()
            : base("name=dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<answer> answer { get; set; }
        public virtual DbSet<child> child { get; set; }
        public virtual DbSet<gender> gender { get; set; }
        public virtual DbSet<image> image { get; set; }
        public virtual DbSet<question> question { get; set; }
        public virtual DbSet<question_type> question_type { get; set; }
        public virtual DbSet<researcher> researcher { get; set; }
        public virtual DbSet<school_type> school_type { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<test> test { get; set; }
        public virtual DbSet<test_instance> test_instance { get; set; }
    }
}
