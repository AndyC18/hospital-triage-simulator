using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealNEA;
using RealNEABackUp.Services;

public class RealTimeScheduling
{
    private HospitalsContext DbContext;
    private PriorityQueue patientQueue;
    private PatientDoctorCompatibility compatibilities;
    private Timer schedulingTimer;
    public object _lock; //acts as a key, only 1 instance can use it at a time
    public AppointmentGenerator appointmentGenerator;

    

    public RealTimeScheduling(HospitalsContext context, PriorityQueue queue, PatientDoctorCompatibility compatibility, object sharedLock)
    {
        DbContext = context;
        patientQueue = queue;
        compatibilities = compatibility;
        _lock = sharedLock;
        AppointmentGenerator _appointmentGenerator = new AppointmentGenerator(DbContext, patientQueue, compatibilities);
        appointmentGenerator = _appointmentGenerator;

        Console.WriteLine("\nStarting simulation...");

        //method to run, state, start time, time period
        schedulingTimer = new Timer(RunSchedulingCycle, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(50)); 
    }

    public void RunSchedulingCycle(object a) //timer method requires a parameter usually to give it context
    {
        //only 1 instance of the cycle can run at a time 
        lock (_lock)
        {   
            //updates all patient doctor compatibilities before appointment scheduling
            compatibilities.UpdateCompatibilities();
            appointmentGenerator.GenerateBatchOfAppointments();
        }
    }   

    public void stopScheduling()
    {
        schedulingTimer.Dispose();
    }

}