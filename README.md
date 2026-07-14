Hospital A&E Triage & Scheduling Simulator

A console-based simulation system modelling how a hospital A&E department could triage patients, match them to suitable doctors, and schedule appointments in real time — while also acting as a stress-testing tool for hospital staffing decisions.

The Problem

A&E departments often struggle with long, unpredictable wait times and scheduling that doesn't account for patient urgency, doctor workload fairness, or doctor specialisation. Existing systems (e.g. NHS 111) triage patients well but don't model staffing or give hospitals a way to stress-test how they'd cope with a surge in patients (e.g. during a flu outbreak).

This project explores whether a combination of priority scheduling, bipartite graph matching, and real-time simulation can produce a fairer, more efficient, and more predictable system — for both patients and doctors.

Full analysis included interviews with a practising nurse and a patient with recurring A&E experience, whose feedback directly shaped the design (see the NEA write-up for details).

Key Features


Patient triage via decision tree — patients answer a sequence of symptom questions; each path deterministically produces a severity score and symptom list. Emergency symptoms short-circuit the tree and flag the patient immediately.
Priority queue scheduling — patients are ordered by a weighted score combining severity, wait time, and age (young/elderly patients weighted higher), with severity as a tiebreaker. Low-severity patients are automatically referred to a GP instead of being queued.
Doctor-patient compatibility scoring — modelled as a weighted bipartite graph. Scores factor in symptom match quality, doctor hours worked (to protect against overworking staff), and how many doctors are qualified to treat a given patient (rarer specialists are prioritised for patients who need them).
Real-time appointment generator — runs on a timer, checks doctor availability, picks the best compatible patient for each free doctor, and schedules appointments without double-booking or overrunning hospital closing hours.
Patient arrival simulator — generates patients with randomised symptoms, ages, and names at regular intervals, allowing the whole system to be used as a stress-testing model (e.g. simulate a surge and see how the queue and staffing cope).
Concurrency handling — a shared lock prevents the scheduler and patient simulator from corrupting shared data when both try to read/write the database at the same time.
SQLite database via EF Core — patients, doctors, symptoms, compatibilities, and appointments are fully modelled and normalised (e.g. PatientSymptom and DoctorSymptom join tables resolve the many-to-many relationships).


Tech Stack


C# / .NET
Entity Framework Core (SQLite) for data modelling and persistence
LINQ for querying and data manipulation


Project Structure

Models/                 Data models (Patient, Doctor, Symptom, Appointment, Compatibility, etc.)
Data/Seeders/           Database initialisation and seeding (doctors, symptoms, triage tree)
Services/               Core logic — priority queue, compatibility scoring, appointment generation
Services/PatientTriage/ Decision tree traversal and patient detail collection
Prototype/              Early prototype classes used during design/testing before the full system
Program.cs              Entry point / interactive console loop
DisplayFunctions.cs     Console output helpers (queue view, appointments, wait times)

Design Highlights


Bipartite graph matching: doctors and patients are modelled as two node sets, with compatibility scores as weighted edges — informed by research into graph colouring and scheduling algorithms.
Deterministic triage: the same sequence of answers always produces the same severity score, which was a deliberate design goal for consistency and fairness across patients.
Fairness-aware compatibility scoring: doctors who've worked longer hours are deprioritised for new appointments, directly addressing feedback from the nurse interview about uneven shift distribution.


Known Limitations

(identified during testing — see NEA "Evaluation" section for full detail)


Doctor hours worked don't always update correctly in the database after very short appointments.
Input validation has gaps for some extreme/edge-case inputs (e.g. very large numeric values).
Patient symptom generation is fully random and doesn't model real-world symptom clustering (e.g. nausea and vomiting co-occurring).
No handling for doctor breaks — the model currently assumes doctors are schedulable all day.


Possible Future Improvements


Fix the doctor-hours update bug.
Add stricter input validation and bounds checking.
Model symptom correlation for more realistic patient generation.
Add scheduled doctor break periods.
Add a Python-based analysis layer over the generated SQLite data (e.g. wait time distributions, doctor utilisation over a simulated day) to complement the C# simulation with statistical/visual analysis.


Background

This was submitted as my NEA for AQA A-Level Computer Science. The full write-up — including stakeholder interviews, requirements analysis, algorithm comparisons, database design, and testing against 30+ objectives — is included in this repo (NEA_writeup.pdf — see project root).
