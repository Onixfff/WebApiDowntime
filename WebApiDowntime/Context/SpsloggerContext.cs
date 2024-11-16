using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using WebApiDowntime.Models;

namespace WebApiDowntime.Context;

public partial class SpsloggerContext : DbContext
{
    public SpsloggerContext()
    {
    }

    public SpsloggerContext(DbContextOptions<SpsloggerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ComparisonResult> ComparisonResults { get; set; }

    public virtual DbSet<Configtable> Configtables { get; set; }

    public virtual DbSet<Diagram> Diagrams { get; set; }

    public virtual DbSet<Diagramm> Diagramms { get; set; }

    public virtual DbSet<Downtime> Downtimes { get; set; }

    public virtual DbSet<ErrorMa> ErrorMas { get; set; }

    public virtual DbSet<Ididle> Ididles { get; set; }

    public virtual DbSet<Mixreport> Mixreports { get; set; }

    public virtual DbSet<Opcserverconfigtable> Opcserverconfigtables { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productsconfigtable> Productsconfigtables { get; set; }

    public virtual DbSet<Productstable> Productstables { get; set; }

    public virtual DbSet<Recepttime> Recepttimes { get; set; }

    public virtual DbSet<Suppliertable> Suppliertables { get; set; }

    public virtual DbSet<Triggertable> Triggertables { get; set; }

    public virtual DbSet<Verpackung> Verpackungs { get; set; }

    public virtual DbSet<Zugang> Zugangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.100.100;database=spslogger;user=D_user;password=Aeroblock12345%", ServerVersion.Parse("8.0.22-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<ComparisonResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comparison_results");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data52)
                .HasMaxLength(64)
                .HasColumnName("data_52");
            entity.Property(e => e.Difference)
                .HasColumnType("time")
                .HasColumnName("difference");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Processed)
                .HasDefaultValueSql("'0'")
                .HasColumnName("processed");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Table1Id).HasColumnName("table1_id");
            entity.Property(e => e.Table2Id).HasColumnName("table2_id");
        });

        modelBuilder.Entity<Configtable>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("configtable");

            entity.HasIndex(e => e.Position, "Position");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.AccessPath)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.DataType).HasDefaultValueSql("'2'");
            entity.Property(e => e.EngUnit)
                .HasMaxLength(40)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Group)
                .HasMaxLength(255)
                .HasDefaultValueSql("'DefaultGroup'");
            entity.Property(e => e.LocalizedName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Multiplier).HasDefaultValueSql("'1'");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.OpcserverDbid)
                .HasDefaultValueSql("'0'")
                .HasColumnName("OPCServerDBID");
            entity.Property(e => e.Opctag)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("OPCTag");
            entity.Property(e => e.ProductColumnId)
                .HasDefaultValueSql("'-1'")
                .HasColumnName("ProductColumnID");
            entity.Property(e => e.Remarks).HasColumnType("text");
            entity.Property(e => e.Visible)
                .IsRequired()
                .HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Diagram>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("diagram");

            entity.HasIndex(e => e.Timestamp, "Timestamp");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Data105).HasColumnName("Data_105");
            entity.Property(e => e.Data106).HasColumnName("Data_106");
            entity.Property(e => e.Data107).HasColumnName("Data_107");
            entity.Property(e => e.Data108).HasColumnName("Data_108");
            entity.Property(e => e.Data109).HasColumnName("Data_109");
            entity.Property(e => e.Data110).HasColumnName("Data_110");
            entity.Property(e => e.Data111).HasColumnName("Data_111");
            entity.Property(e => e.Data148).HasColumnName("Data_148");
            entity.Property(e => e.Data245).HasColumnName("Data_245");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Diagramm>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity
                .ToTable("diagramm")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Timestamp, "Timestamp");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Data111).HasColumnName("Data_111");
            entity.Property(e => e.Data112).HasColumnName("Data_112");
            entity.Property(e => e.Data113).HasColumnName("Data_113");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Downtime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("downtime")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Recept, "receptTime_idx");

            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Difference).HasColumnType("time");
            entity.Property(e => e.IdIdle).HasColumnName("idIdle");
            entity.Property(e => e.Recept).HasMaxLength(145);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.TimestampEnd).HasColumnType("datetime");
            entity.Property(e => e.isUpdate)
    .HasDefaultValueSql("'1'")
    .HasColumnName("isUpdate");

            entity.HasOne(d => d.ReceptNavigation).WithMany(p => p.Downtimes)
                .HasForeignKey(d => d.Recept)
                .HasConstraintName("receptTime");
        });

        modelBuilder.Entity<ErrorMa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("error_mas")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Recepte, "Index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comments)
                .HasMaxLength(245)
                .HasColumnName("comments");
            entity.Property(e => e.DataErr)
                .HasColumnType("datetime")
                .HasColumnName("data_err");
            entity.Property(e => e.Recepte)
                .HasMaxLength(145)
                .HasColumnName("recepte")
                .UseCollation("utf8_bin");
            entity.Property(e => e.SumEr).HasColumnName("sum_er");
        });

        modelBuilder.Entity<Ididle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("ididles")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Mixreport>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("mixreport");

            entity.HasIndex(e => e.Data52, "Data_52");

            entity.HasIndex(e => e.Timestamp, "Timestamp");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Data1).HasColumnName("Data_1");
            entity.Property(e => e.Data10).HasColumnName("Data_10");
            entity.Property(e => e.Data100).HasColumnName("Data_100");
            entity.Property(e => e.Data101).HasColumnName("Data_101");
            entity.Property(e => e.Data102).HasColumnName("Data_102");
            entity.Property(e => e.Data103).HasColumnName("Data_103");
            entity.Property(e => e.Data11).HasColumnName("Data_11");
            entity.Property(e => e.Data115).HasColumnName("Data_115");
            entity.Property(e => e.Data116).HasColumnName("Data_116");
            entity.Property(e => e.Data117).HasColumnName("Data_117");
            entity.Property(e => e.Data118).HasColumnName("Data_118");
            entity.Property(e => e.Data119).HasColumnName("Data_119");
            entity.Property(e => e.Data12).HasColumnName("Data_12");
            entity.Property(e => e.Data120).HasColumnName("Data_120");
            entity.Property(e => e.Data121).HasColumnName("Data_121");
            entity.Property(e => e.Data122).HasColumnName("Data_122");
            entity.Property(e => e.Data123).HasColumnName("Data_123");
            entity.Property(e => e.Data124).HasColumnName("Data_124");
            entity.Property(e => e.Data125).HasColumnName("Data_125");
            entity.Property(e => e.Data126).HasColumnName("Data_126");
            entity.Property(e => e.Data127).HasColumnName("Data_127");
            entity.Property(e => e.Data128).HasColumnName("Data_128");
            entity.Property(e => e.Data129).HasColumnName("Data_129");
            entity.Property(e => e.Data13).HasColumnName("Data_13");
            entity.Property(e => e.Data130).HasColumnName("Data_130");
            entity.Property(e => e.Data131).HasColumnName("Data_131");
            entity.Property(e => e.Data132).HasColumnName("Data_132");
            entity.Property(e => e.Data133).HasColumnName("Data_133");
            entity.Property(e => e.Data134).HasColumnName("Data_134");
            entity.Property(e => e.Data138).HasColumnName("Data_138");
            entity.Property(e => e.Data14).HasColumnName("Data_14");
            entity.Property(e => e.Data145).HasColumnName("Data_145");
            entity.Property(e => e.Data146).HasColumnName("Data_146");
            entity.Property(e => e.Data147).HasColumnName("Data_147");
            entity.Property(e => e.Data151).HasColumnName("Data_151");
            entity.Property(e => e.Data154)
                .HasMaxLength(255)
                .HasColumnName("Data_154");
            entity.Property(e => e.Data155).HasColumnName("Data_155");
            entity.Property(e => e.Data156).HasColumnName("Data_156");
            entity.Property(e => e.Data158).HasColumnName("Data_158");
            entity.Property(e => e.Data159).HasColumnName("Data_159");
            entity.Property(e => e.Data160).HasColumnName("Data_160");
            entity.Property(e => e.Data161).HasColumnName("Data_161");
            entity.Property(e => e.Data162).HasColumnName("Data_162");
            entity.Property(e => e.Data163).HasColumnName("Data_163");
            entity.Property(e => e.Data164).HasColumnName("Data_164");
            entity.Property(e => e.Data165).HasColumnName("Data_165");
            entity.Property(e => e.Data166).HasColumnName("Data_166");
            entity.Property(e => e.Data167).HasColumnName("Data_167");
            entity.Property(e => e.Data168).HasColumnName("Data_168");
            entity.Property(e => e.Data169).HasColumnName("Data_169");
            entity.Property(e => e.Data171).HasColumnName("Data_171");
            entity.Property(e => e.Data172).HasColumnName("Data_172");
            entity.Property(e => e.Data173).HasColumnName("Data_173");
            entity.Property(e => e.Data174).HasColumnName("Data_174");
            entity.Property(e => e.Data175).HasColumnName("Data_175");
            entity.Property(e => e.Data176).HasColumnName("Data_176");
            entity.Property(e => e.Data177).HasColumnName("Data_177");
            entity.Property(e => e.Data178).HasColumnName("Data_178");
            entity.Property(e => e.Data179).HasColumnName("Data_179");
            entity.Property(e => e.Data180).HasColumnName("Data_180");
            entity.Property(e => e.Data181).HasColumnName("Data_181");
            entity.Property(e => e.Data182).HasColumnName("Data_182");
            entity.Property(e => e.Data183).HasColumnName("Data_183");
            entity.Property(e => e.Data184).HasColumnName("Data_184");
            entity.Property(e => e.Data185).HasColumnName("Data_185");
            entity.Property(e => e.Data186).HasColumnName("Data_186");
            entity.Property(e => e.Data187).HasColumnName("Data_187");
            entity.Property(e => e.Data188).HasColumnName("Data_188");
            entity.Property(e => e.Data189).HasColumnName("Data_189");
            entity.Property(e => e.Data190).HasColumnName("Data_190");
            entity.Property(e => e.Data191).HasColumnName("Data_191");
            entity.Property(e => e.Data192).HasColumnName("Data_192");
            entity.Property(e => e.Data193).HasColumnName("Data_193");
            entity.Property(e => e.Data195).HasColumnName("Data_195");
            entity.Property(e => e.Data196).HasColumnName("Data_196");
            entity.Property(e => e.Data198).HasColumnName("Data_198");
            entity.Property(e => e.Data199).HasColumnName("Data_199");
            entity.Property(e => e.Data200).HasColumnName("Data_200");
            entity.Property(e => e.Data201).HasColumnName("Data_201");
            entity.Property(e => e.Data202).HasColumnName("Data_202");
            entity.Property(e => e.Data203).HasColumnName("Data_203");
            entity.Property(e => e.Data204).HasColumnName("Data_204");
            entity.Property(e => e.Data205).HasColumnName("Data_205");
            entity.Property(e => e.Data206).HasColumnName("Data_206");
            entity.Property(e => e.Data207).HasColumnName("Data_207");
            entity.Property(e => e.Data208).HasColumnName("Data_208");
            entity.Property(e => e.Data209).HasColumnName("Data_209");
            entity.Property(e => e.Data21).HasColumnName("Data_21");
            entity.Property(e => e.Data210).HasColumnName("Data_210");
            entity.Property(e => e.Data211).HasColumnName("Data_211");
            entity.Property(e => e.Data212).HasColumnName("Data_212");
            entity.Property(e => e.Data213).HasColumnName("Data_213");
            entity.Property(e => e.Data214).HasColumnName("Data_214");
            entity.Property(e => e.Data215).HasColumnName("Data_215");
            entity.Property(e => e.Data216).HasColumnName("Data_216");
            entity.Property(e => e.Data217).HasColumnName("Data_217");
            entity.Property(e => e.Data218).HasColumnName("Data_218");
            entity.Property(e => e.Data219).HasColumnName("Data_219");
            entity.Property(e => e.Data22).HasColumnName("Data_22");
            entity.Property(e => e.Data222).HasColumnName("Data_222");
            entity.Property(e => e.Data223).HasColumnName("Data_223");
            entity.Property(e => e.Data224).HasColumnName("Data_224");
            entity.Property(e => e.Data225).HasColumnName("Data_225");
            entity.Property(e => e.Data226).HasColumnName("Data_226");
            entity.Property(e => e.Data227).HasColumnName("Data_227");
            entity.Property(e => e.Data228).HasColumnName("Data_228");
            entity.Property(e => e.Data229).HasColumnName("Data_229");
            entity.Property(e => e.Data23).HasColumnName("Data_23");
            entity.Property(e => e.Data230).HasColumnName("Data_230");
            entity.Property(e => e.Data231).HasColumnName("Data_231");
            entity.Property(e => e.Data232).HasColumnName("Data_232");
            entity.Property(e => e.Data233).HasColumnName("Data_233");
            entity.Property(e => e.Data234).HasColumnName("Data_234");
            entity.Property(e => e.Data235).HasColumnName("Data_235");
            entity.Property(e => e.Data236).HasColumnName("Data_236");
            entity.Property(e => e.Data237).HasColumnName("Data_237");
            entity.Property(e => e.Data238).HasColumnName("Data_238");
            entity.Property(e => e.Data239).HasColumnName("Data_239");
            entity.Property(e => e.Data24).HasColumnName("Data_24");
            entity.Property(e => e.Data240).HasColumnName("Data_240");
            entity.Property(e => e.Data241).HasColumnName("Data_241");
            entity.Property(e => e.Data242).HasColumnName("Data_242");
            entity.Property(e => e.Data243).HasColumnName("Data_243");
            entity.Property(e => e.Data244).HasColumnName("Data_244");
            entity.Property(e => e.Data247).HasColumnName("Data_247");
            entity.Property(e => e.Data248).HasColumnName("Data_248");
            entity.Property(e => e.Data249).HasColumnName("Data_249");
            entity.Property(e => e.Data25).HasColumnName("Data_25");
            entity.Property(e => e.Data250).HasColumnName("Data_250");
            entity.Property(e => e.Data251).HasColumnName("Data_251");
            entity.Property(e => e.Data252).HasColumnName("Data_252");
            entity.Property(e => e.Data253).HasColumnName("Data_253");
            entity.Property(e => e.Data254)
                .HasComment("OrderID")
                .HasColumnName("Data_254");
            entity.Property(e => e.Data255)
                .HasComment("RecipeID")
                .HasColumnName("Data_255");
            entity.Property(e => e.Data256)
                .HasDefaultValueSql("'0'")
                .HasColumnName("Data_256");
            entity.Property(e => e.Data26).HasColumnName("Data_26");
            entity.Property(e => e.Data27).HasColumnName("Data_27");
            entity.Property(e => e.Data28).HasColumnName("Data_28");
            entity.Property(e => e.Data29).HasColumnName("Data_29");
            entity.Property(e => e.Data30).HasColumnName("Data_30");
            entity.Property(e => e.Data31).HasColumnName("Data_31");
            entity.Property(e => e.Data32).HasColumnName("Data_32");
            entity.Property(e => e.Data34).HasColumnName("Data_34");
            entity.Property(e => e.Data35).HasColumnName("Data_35");
            entity.Property(e => e.Data37).HasColumnName("Data_37");
            entity.Property(e => e.Data38).HasColumnName("Data_38");
            entity.Property(e => e.Data39).HasColumnName("Data_39");
            entity.Property(e => e.Data40).HasColumnName("Data_40");
            entity.Property(e => e.Data41).HasColumnName("Data_41");
            entity.Property(e => e.Data42).HasColumnName("Data_42");
            entity.Property(e => e.Data43).HasColumnName("Data_43");
            entity.Property(e => e.Data44).HasColumnName("Data_44");
            entity.Property(e => e.Data45).HasColumnName("Data_45");
            entity.Property(e => e.Data46).HasColumnName("Data_46");
            entity.Property(e => e.Data47).HasColumnName("Data_47");
            entity.Property(e => e.Data48).HasColumnName("Data_48");
            entity.Property(e => e.Data49).HasColumnName("Data_49");
            entity.Property(e => e.Data50).HasColumnName("Data_50");
            entity.Property(e => e.Data51).HasColumnName("Data_51");
            entity.Property(e => e.Data52).HasColumnName("Data_52");
            entity.Property(e => e.Data6).HasColumnName("Data_6");
            entity.Property(e => e.Data7).HasColumnName("Data_7");
            entity.Property(e => e.Data8).HasColumnName("Data_8");
            entity.Property(e => e.Data9).HasColumnName("Data_9");
            entity.Property(e => e.Data96).HasColumnName("Data_96");
            entity.Property(e => e.Data97).HasColumnName("Data_97");
            entity.Property(e => e.Data98).HasColumnName("Data_98");
            entity.Property(e => e.Data99).HasColumnName("Data_99");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Opcserverconfigtable>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("opcserverconfigtable");

            entity.HasIndex(e => e.Position, "Position");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.OpcdataAccessType)
                .HasDefaultValueSql("'1'")
                .HasColumnName("OPCDataAccessType");
            entity.Property(e => e.RemoteMachineName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.ServerClassId)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("ServerClassID");
            entity.Property(e => e.ServerProgId)
                .HasMaxLength(255)
                .HasDefaultValueSql("'OPC.SimaticNET'")
                .HasColumnName("ServerProgID");
            entity.Property(e => e.UpdateRate).HasDefaultValueSql("'500'");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.Timestamp, "Timestamp");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Productsconfigtable>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity
                .ToTable("productsconfigtable")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Position, "Position");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.DataType).HasDefaultValueSql("'2'");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Productstable>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("productstable");

            entity.HasIndex(e => e.Position, "Position");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Product1)
                .HasMaxLength(255)
                .HasColumnName("Product_1");
            entity.Property(e => e.Product2)
                .HasMaxLength(255)
                .HasColumnName("Product_2");
            entity.Property(e => e.Product3)
                .HasMaxLength(255)
                .HasColumnName("Product_3");
            entity.Property(e => e.Product4).HasColumnName("Product_4");
            entity.Property(e => e.Product5).HasColumnName("Product_5");
            entity.Property(e => e.Product6).HasColumnName("Product_6");
        });

        modelBuilder.Entity<Recepttime>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity
                .ToTable("recepttime")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Name)
                .HasMaxLength(145)
                .UseCollation("utf8_bin");
            entity.Property(e => e.Time)
                .HasDefaultValueSql("'07:30:00'")
                .HasColumnType("time");
        });

        modelBuilder.Entity<Suppliertable>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("suppliertable");

            entity.HasIndex(e => e.Position, "Position");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Location)
                .HasMaxLength(60)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Remarks).HasColumnType("text");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasColumnName("ZIPCode");
        });

        modelBuilder.Entity<Triggertable>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity
                .ToTable("triggertable")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Position, "Position");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.AccessPath)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.DataType).HasDefaultValueSql("'2'");
            entity.Property(e => e.Group)
                .HasMaxLength(255)
                .HasDefaultValueSql("'DefaultGroup'");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.OpcserverDbid)
                .HasDefaultValueSql("'0'")
                .HasColumnName("OPCServerDBID");
            entity.Property(e => e.Opctag)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("OPCTag");
            entity.Property(e => e.SourceColumn)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.SourceTable)
                .HasMaxLength(255)
                .HasDefaultValueSql("'ProductsTable'");
        });

        modelBuilder.Entity<Verpackung>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("verpackung");

            entity.HasIndex(e => e.Timestamp, "Timestamp");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Data122)
                .HasMaxLength(255)
                .HasColumnName("Data_122");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Zugang>(entity =>
        {
            entity.HasKey(e => e.Dbid).HasName("PRIMARY");

            entity.ToTable("zugang");

            entity.HasIndex(e => e.Timestamp, "Timestamp");

            entity.Property(e => e.Dbid).HasColumnName("DBID");
            entity.Property(e => e.Data135).HasColumnName("Data_135");
            entity.Property(e => e.Data136).HasColumnName("Data_136");
            entity.Property(e => e.Data137).HasColumnName("Data_137");
            entity.Property(e => e.Data246).HasColumnName("Data_246");
            entity.Property(e => e.Data53).HasColumnName("Data_53");
            entity.Property(e => e.Data54).HasColumnName("Data_54");
            entity.Property(e => e.Data55).HasColumnName("Data_55");
            entity.Property(e => e.Data56).HasColumnName("Data_56");
            entity.Property(e => e.Data57).HasColumnName("Data_57");
            entity.Property(e => e.Data58).HasColumnName("Data_58");
            entity.Property(e => e.Data61).HasColumnName("Data_61");
            entity.Property(e => e.Data62).HasColumnName("Data_62");
            entity.Property(e => e.Data86).HasColumnName("Data_86");
            entity.Property(e => e.Data87).HasColumnName("Data_87");
            entity.Property(e => e.Data88).HasColumnName("Data_88");
            entity.Property(e => e.Data89).HasColumnName("Data_89");
            entity.Property(e => e.Data90).HasColumnName("Data_90");
            entity.Property(e => e.Data91).HasColumnName("Data_91");
            entity.Property(e => e.Data92).HasColumnName("Data_92");
            entity.Property(e => e.Data93).HasColumnName("Data_93");
            entity.Property(e => e.Data94).HasColumnName("Data_94");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'0000-00-00 00:00:00'")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
