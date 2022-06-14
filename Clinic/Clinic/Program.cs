using System;
using static System.Console;
using System.Collections.Generic;
using static System.Math;
namespace ConsoleApp1
{
    struct Patient
    {
        private int Index;
        private string Name;
        private string SecondName;
        private string Surname;
        private DateTime DateOfBirth;
        private string Diagnose;
        public Patient(int Index, string Name, string SecondName, string
        Surname, DateTime DateOfBirth, string Diagnose)
        {
            this.Index = Index;
            this.Name = Name;
            this.SecondName = SecondName;
            this.Surname = Surname;
            this.DateOfBirth = DateOfBirth;
            this.Diagnose = Diagnose;
        }
        public int getIndex() => Index;
        public string getName() => Name;
        public string getSecondName() => SecondName;
        public string getSurname() => Surname;
        public DateTime getDateOfBirth() => DateOfBirth;
        public string getDiagnose() => Diagnose;
        public void printPatient()
        {
            Write($"{Index,3}");
            Write($"{Name,15},");
            Write($"{SecondName,15},");
            Write($"{Surname,15},");
            Write($"{DateOfBirth.ToString("dd/MM/yyyy"),12},");
            Write($"{Diagnose,8}.");
        }
    }
    struct Doctor
    {
        private int Index;
        private string Name;
        private string SecondName;
        private string Surname;
        private DateTime DateOfBirth;
        private string Posada;
        private string Specialisation;
        public Doctor(int Index, string Name, string SecondName, string
        Surname, DateTime DateOfBirth, string Posada, string Specialisation)
        {
            this.Index = Index;
            this.Name = Name;
            this.SecondName = SecondName;
            this.Surname = Surname;
            this.DateOfBirth = DateOfBirth;
            this.Posada = Posada;
            this.Specialisation = Specialisation;
        }
        public int getIndex() => Index;
        public string getName() => Name;
        public string getSecondName() => SecondName;
        public string getSurname() => Surname;
        public DateTime getDateOfBirth() => DateOfBirth;
        public string getPosada() => Posada;
        public string getSpecialisation() => Specialisation;
        public void printDoctor()
        {
            Write($"{Index,3}");
            Write($"{Name,15},");
            Write($"{SecondName,15},");
            Write($"{Surname,15},");
            Write($"{DateOfBirth.ToString("dd/MM/yyyy"),12},");
            Write($"{Posada,8},");
            Write($"{Specialisation,15}.");
        }
    }
    struct Priyom
    {
        private int DoctorIndex;
        private int PatientIndex;
        private string DatePriyom;
        private int PriyomIndex;
        public Priyom(int PriyomIndex, int DoctorIndex, int PatientIndex,
        string DatePriyom)
        {
            this.DoctorIndex = DoctorIndex;
            this.PatientIndex = PatientIndex;
            this.DatePriyom = DatePriyom;
            this.PriyomIndex = PriyomIndex;
        }
        public int getDoctorIndex() => DoctorIndex;
        public int getPatientIndex() => PatientIndex;
        public string getDatePriyom() => DatePriyom;
        public int getPriyomIndex() => PriyomIndex;
        public void printPriyom()
        {
            Write($"{PriyomIndex,11}");
            Write($"{PatientIndex,12},");
            Write($"{DoctorIndex,13},");
            WriteLine($"{DatePriyom,15}.");
        }
    }
    class GenerateDataOfPriyom
    {
        Random rnd = new Random();
        private string[] Dates =
        {
"01.01.2021","02.01.2021", "03.01.2021","04.01.2021",
"05.01.2021", "06.01.2021",
"07.01.2021","08.01.2021","09.01.2021",
"10.01.2021", "11.01.2021","12.01.2021",
"13.01.2021","14.01.2021",
"15.01.2021", "01.01.2021", "01.01.2021"
};
        public int genDoctor(List<Doctor> doc)
        {
            int Doctor = rnd.Next(0, doc.Count);
            return Doctor;
        }
        public int genPatient(List<Patient> pat)
        {
            int Patient = rnd.Next(0, pat.Count);
            return Patient;
        }
        public string genDatePriyom() => Dates[rnd.Next(0, Dates.Length)];
        public int getDatesCount() => Dates.Length;
    }
    class GenerateDataOfPatient
    {
        Random rnd = new Random();
        private string[] Surnames =
        {
"Kucherenko","Bondarchuk", "Korniychuk","Makovetskiy",
"Bodakwa", "Gavrylenko", "Gura","Dryga","Zlenko",
"Kosenko", "Lunev","Mazniy", "Nikisha","Snigir",
"Regesha", "Khandoknin", "Jakovchuk"
};
        private string[] Names =
        {
"Daniil", "Artem","Alexander", "Sergii",
"Andriy", "Ivan","Vladislav", "Dmytro",
"Igor","Kirilo","Mykola","Stepan","Kirill",
};
        private string[] SecondNames =
        {
"Platonovych", "Mykhaylovych", "Valeriyovych",
"Vitaliyovich", "Oleksandrovych", "Artemovych",
"Andriyovych", "Sergiovych","Vadymovych",
"Volodymyrovych", "Olegovych", "Igorovych",
};
        private enum Diagnose
        {
            Fever, Corona, Flu
        }
        public DateTime GenerateDateOfBirth()
        {
            Random rnd = new Random();
            return new DateTime(rnd.Next(1900, 2020), rnd.Next(1, 13),
            rnd.Next(1, 29));
        }
        public string getName() => Names[rnd.Next(0, Names.Length)];
        public int getNamesCount() => Names.Length;
        public string getSecondName() => SecondNames[rnd.Next(0,
        SecondNames.Length)];
        public string getSurname() => Surnames[rnd.Next(0, Surnames.Length)];
        public int getSurnamesCount() => Surnames.Length;
        public string getDiagnose() =>
        Convert.ToString((Diagnose)rnd.Next(3));
    }
    class GenerateDataOfDoctor
    {
        Random rnd = new Random();
        private string[] Surnames =
        {
"Pisotska", "Horak", "Kosheva","Nebesna",
"Kirilchuk", "Jolobytska","Eaphimova",
};
        private string[] Names =
        {
"Oleksandra","Sophia", "Sonia",
"Margaryta", "Anna", "Nastia",
};
        private string[] SecondNames =
        {
"Oleksandrivna", "Evgenivna", "Vitaliivna",
"Arturivna","Petrivna","Mykolaivna",
};
        private enum Posada
        {
            Doctor, Nurse, Sanitar
        }
        private enum Specialisation
        {
            Infectionist, Rentgenologist, Therapevt
        }
        public DateTime GenerateDateOfBirth()
        {
            Random rnd = new Random();
            return new DateTime(rnd.Next(1900, 2020), rnd.Next(1, 13),
            rnd.Next(1, 29));
        }
        public string getSecondName() => SecondNames[rnd.Next(0,
        SecondNames.Length)];
        public string getName() => Names[rnd.Next(0, Names.Length)];
        public int getNamesCount() => Names.Length;
        public string getSurname() => Surnames[rnd.Next(0, Surnames.Length)];
        public int getSurnamesCount() => Surnames.Length;
        public string getPosada() => Convert.ToString((Posada)rnd.Next(3));
        public string getSpecialisation() =>
        Convert.ToString((Specialisation)rnd.Next(3));
    }
    class MainClass
    {
        static Random rnd = new Random();
        static GenerateDataOfPatient genpatient = new GenerateDataOfPatient();
        static GenerateDataOfDoctor gendoctor = new GenerateDataOfDoctor();
        static GenerateDataOfPriyom genpriyom = new GenerateDataOfPriyom();
        static void Main(string[] args)
        {
            List<Patient> pat;
            List<Doctor> doc;
            List<Priyom> pri;
            (pat, doc, pri) = CreateDatabaseForHospital();
            PrintPatient(pat);
            PrintDoctor(doc);
            printPriyom(pri);
            PatientDiagnose(pat);
            DoctorDate(pri, pat);
            DoctorsofPatient(pri, doc);
        }
        static (List<Patient>, List<Doctor>, List<Priyom>)
        CreateDatabaseForHospital()
        {
            List<Patient> Patients = new List<Patient>();
            List<Doctor> Doctors = new List<Doctor>();
            List<Priyom> Priyoms = new List<Priyom>();
            for (byte i = 0; i < genpatient.getSurnamesCount(); i++)
            {
                Patients.Add(
                new Patient(
                i,
                genpatient.getSurname(),
                genpatient.getName(),
                genpatient.getSecondName(),
                genpatient.GenerateDateOfBirth(),
                genpatient.getDiagnose()
                )
                );
            }
            for (byte i = 0; i < gendoctor.getSurnamesCount(); i++)
            {
                Doctors.Add(new Doctor(
                i,
                gendoctor.getSurname(),
                gendoctor.getName(),
                gendoctor.getSecondName(),
                gendoctor.GenerateDateOfBirth(),
                gendoctor.getPosada(),
                gendoctor.getSpecialisation()));
            }
            for (byte i = 0; i < genpriyom.getDatesCount(); i++)
            {
                Priyoms.Add(
                new Priyom(i,
                genpriyom.genDoctor(Doctors),
                genpriyom.genPatient(Patients),
                genpriyom.genDatePriyom()
                )
                );
            }
            return (Patients, Doctors, Priyoms);
        }
        static void PrintPatient(List<Patient> ListOfPatient)
        {
            WriteLine($"{"Patients:",51}");
            foreach (Patient P in ListOfPatient)
            {
                P.printPatient();
                WriteLine();
            }
            WriteLine();
        }
        static void PrintDoctor(List<Doctor> ListOfDoctor)
        {
            WriteLine($"{"Doctors:",51}");
            foreach (Doctor D in ListOfDoctor)
            {
                D.printDoctor();
                WriteLine();
            }
            WriteLine();
        }
        static void printPriyom(List<Priyom> ListOfPriyom)
        {
            WriteLine("Index Priyomu| Index Patient| Index Doctor| Date Priyomu");
        foreach (Priyom Y in ListOfPriyom)
            {
                Y.printPriyom();
                WriteLine();
            }
            WriteLine();
        }
        static void PatientDiagnose(List<Patient> pat)
        {
            Write("Enter number of patient to get diagnose: ");
            int PatientIndex = int.Parse(ReadLine());
            WriteLine("Diagnose: " + $"{pat[PatientIndex].getDiagnose()}");
        }
        static void DoctorDate(List<Priyom> pri, List<Patient> pat)
        {
            Write("Enter number of doctor to get patients: ");
            int DoctorIndex = int.Parse(ReadLine());
            Write("Enter date of priyom: ");
            string Date = ReadLine();
            var priyomsByDoctorAndDate = pri.FindAll(p => {
                return p.getDoctorIndex() == DoctorIndex && p.getDatePriyom()
                == Date;
            });
            WriteLine("Patients: ");
            for (int i = 0; i < priyomsByDoctorAndDate.Count; i++)
            {
                int ip = priyomsByDoctorAndDate[i].getPatientIndex();
                pat[ip].printPatient();
                Write("\n");
            }
        }
        static void DoctorsofPatient(List<Priyom> pri, List<Doctor> doc)
        {
            WriteLine("Enter number of patient to get doctors: ");
            int PatientIndex = int.Parse(ReadLine());
            List<Priyom> priyomofpatient = pri.FindAll(p =>
            p.getPatientIndex() == PatientIndex);
            WriteLine("Doctors: ");
            for (int i = 0; i < priyomofpatient.Count; i++)
            {
                int id = priyomofpatient[i].getDoctorIndex();
                doc[id].printDoctor();
                Write("\n");
            }
        }
    }
}

