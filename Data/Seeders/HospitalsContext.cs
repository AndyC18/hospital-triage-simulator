using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RealNEA;

public class HospitalsContext : DbContext
{
    //defines the relationships in the database
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=hospitals.db"); //creates SQLite database file
    }


    //tables
    public DbSet<PatientClass> Patients { get; set; }
    public DbSet<SymptomClass> Symptoms { get; set; }
    public DbSet<PatientSymptom> PatientSymptoms { get; set; }
    public DbSet<SymptomQuestion> SymptomQuestions { get; set; }
    public DbSet<SymptomAnswer> SymptomAnswers { get; set; }

    public DbSet<DoctorClass> Doctors { get; set; }
    public DbSet<DoctorSymptomClass> DoctorSymptoms { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<CompatibilityClass> Compatibilities { get; set; }

    //creates the relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //defining pks

        modelBuilder.Entity<PatientClass>()
            .HasKey(patient => patient.PatientId);
        modelBuilder.Entity<PatientClass>()
            .Property(patient => patient.PatientId)
            .ValueGeneratedOnAdd(); //autoincrements patientId

        modelBuilder.Entity<SymptomClass>()
            .HasKey(symptom => symptom.SymptomId);
        modelBuilder.Entity<SymptomClass>()
            .Property(symptom => symptom.SymptomId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<SymptomQuestion>()
            .HasKey(symptomquestion => symptomquestion.SymptomQuestionId);
        modelBuilder.Entity<SymptomQuestion>()
            .Property(SymptomQuestion => SymptomQuestion.SymptomQuestionId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<SymptomAnswer>()
            .HasKey(symptomAnswer => symptomAnswer.SymptomAnswerId);
        modelBuilder.Entity<SymptomAnswer>()
            .Property(SymptomAnswer => SymptomAnswer.SymptomAnswerId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<PatientSymptom>()
        .HasKey(patientsymptom => new { patientsymptom.PatientId, patientsymptom.SymptomId }); //the composite primary key

        modelBuilder.Entity<DoctorClass>()
            .HasKey(Doctor => Doctor.DoctorId);
        modelBuilder.Entity<DoctorClass>()
            .Property(doctor => doctor.DoctorId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<DoctorSymptomClass>()
        .HasKey(doctorsymptom => new { doctorsymptom.DoctorId, doctorsymptom.SymptomId });

        modelBuilder.Entity<Appointment>()
            .HasKey(Appointment => Appointment.AppointmentId);
        modelBuilder.Entity<Appointment>()
            .Property(Appointment => Appointment.AppointmentId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CompatibilityClass>()
            .HasKey(compatibility => new { compatibility.DoctorId, compatibility.PatientId });

        //defining relationships 

        //each symptom answer has a symptom question it came from, and each question has many answers, the foreign key is the symptom questionID
        modelBuilder.Entity<SymptomAnswer>()
        .HasOne(answer => answer.SymptomQuestion)
        .WithMany(question => question.SymptomAnswers)
        .HasForeignKey(answer => answer.SymptomQuestionId);

        //each symptom answer has a nextquestion, symptom answer also has fk of NextQuestionId
        modelBuilder.Entity<SymptomAnswer>()
        .HasOne(answer => answer.NextQuestion)
        .WithMany() // not a collection
        .HasForeignKey(answer => answer.NextQuestionID);

        //every symptomAnswer has a symptom, each symptom has many symptomanswers associated with it. SymptomAnswer has a fk of the SymptomClassId
        modelBuilder.Entity<SymptomAnswer>()
        .HasOne(answer => answer.Symptom)
        .WithMany(symptom => symptom.SymptomAnswers)
        .HasForeignKey(answer => answer.SymptomClassId)
        .IsRequired(false); //not every answer leads to a symptom


        //each patient symptom has a symptom, each symptom has many patient symptoms, patient symptom takes fk SymptomId
        modelBuilder.Entity<PatientSymptom>()
        .HasOne(patientsymptom => patientsymptom.Symptom)
        .WithMany(symptom => symptom.PatientSymptoms)
        .HasForeignKey(PatientSymptom => PatientSymptom.SymptomId);

        //each patientSymptom has a patient, each patient has many patientsymptoms, fk of patientId
        modelBuilder.Entity<PatientSymptom>()
        .HasOne(patientsymptom => patientsymptom.Patient)
        .WithMany(patient => patient.PatientSymptoms)
        .HasForeignKey(patientsymptom => patientsymptom.PatientId);

        //every doctorsymptom has a doctor, each doctor has many doctor symptoms, FK doctor ID
        modelBuilder.Entity<DoctorSymptomClass>()
        .HasOne(doctorsymptom => doctorsymptom.Doctor)
        .WithMany(doctor => doctor.doctorSymptoms)
        .HasForeignKey(doctorsymptom => doctorsymptom.DoctorId);

        //every doctor symptom has a symptom, each symptom has many doctor IDs, FK symptom ID
        modelBuilder.Entity<DoctorSymptomClass>()
        .HasOne(doctorsymptom => doctorsymptom.Symptom)
        .WithMany(symptom => symptom.DoctorSymptoms)
        .HasForeignKey(doctorsymptom => doctorsymptom.SymptomId);

        //every appointment has a patient, each patient has many appointments, FK patient ID
        modelBuilder.Entity<Appointment>()
        .HasOne(appointment => appointment.Patient)
        .WithMany(patient => patient.Appointments)
        .HasForeignKey(appointment => appointment.PatientId);

        //every appointment has a doctor, each doctor has many appointments, FK doctor ID
        modelBuilder.Entity<Appointment>()
        .HasOne(appointment => appointment.Doctor)
        .WithMany(doctor => doctor.Appointments)
        .HasForeignKey(appointment => appointment.DoctorId);

        //every compatibility relationship has a doctor, each doctor has relationships with many patients, FK doctor ID
        modelBuilder.Entity<CompatibilityClass>()
        .HasOne(c => c.Doctor)
        .WithMany(d => d.linkedPatients)
        .HasForeignKey(c => c.DoctorId);

        //every compatibility relationship has a patient, each patient has relationships with many doctors, FK patient ID
        modelBuilder.Entity<CompatibilityClass>()
        .HasOne(c => c.Patient)
        .WithMany(d => d.linkedDoctors)
        .HasForeignKey(c => c.PatientId);

    }
}