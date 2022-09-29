using Company.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EF
{
    class Program
    {

        static void Main(string[] args)
        {

            using (var context = new WoningContext())
            {

                //InsertWoning(context); //Insert woning id 4 
                //InsertData(context);
                //InsertRelatedData(context);//voegt bewoners toe aan woning id 4
                //UpdateRelatedData(context);
                GetWoningen(context);
            }
            //DisconnectedUpdateVoorbeeld1();//update bewoners in woning id 4
        }




        //Connected scenario
        private static void UpdateRelatedData(WoningContext context)
        {
            var woning = context.Woningen.Include(w => w.Bewoners)
                            .FirstOrDefault(w => w.Id == 4);
            var bewoner = woning.Bewoners[0];
            bewoner.Naam = "Astrid";
            context.Bewoners.Update(bewoner);
            context.SaveChanges();
        }

        //Disconnected scenario
        private static void DisconnectedUpdateVoorbeeld1()
        {
            //Call naar de server om bewoners op te halen 
            //van een bestaand studentenhuis

            List<Bewoner> bewoners; //wordt gestuurd naar de client/View

            using (var ctx = new WoningContext())
            {
                var woningen = ctx.Woningen.Include(w => w.Bewoners)
                                .FirstOrDefault(w => w.Id == 4);

                bewoners = woningen.Bewoners;
            }

            //De connectie met de DB is gesloten en de lijst met bewoners van de woning met PK=5
            //wordt getoond op het scherm bij de client.


            //De client veranderd de naam van de eerste bewoner van het studentenhuis 
            //want deze persoon gaat het huis uit en een ander komt er voor in de plaats.
            //Client veranderd de naam in de browser. 
            bewoners[0].Naam = "Nieuwe student"; //Dit is bijv een tekstveld dat uigelezen wordt en 
                                                 //via een HTTPPost de controller binnenkomt

            //Laten we aannemen dat hier ook nog een Repository zit dat uiteindelijk bij de hieronderstaande 
            //code terecht komt

            using (var context = new WoningContext())
            {
                context.Entry(bewoners[0]).State = EntityState.Modified;
                
                context.SaveChanges();
            }
        }

        //disconnected en connected scenario
        //Nieuwe woning plus de geralateerde Bewoner(s) toevoegen
        //Dit kan binnen één context en geldt voor zowel dis- als connected scenario
        private static void InsertData(WoningContext context)
        {
            context.Woningen.Add(
                new Woning
                {
                    Naam = "leeg huis",
                    Huisnummer = 12,
                    Bewoners = new List<Bewoner>
                        {
                            new Bewoner{Naam="Jopie"},
                            new Bewoner{Naam="Jan"}
                        }
                });
            context.SaveChanges();
        }

        //connected scenario
        //Bij een bestaand Woning gerelateerde data toevoegen
        private static void InsertRelatedData(WoningContext context)
        {
            var woning = context.Woningen.FirstOrDefault(w => w.Id == 4);
            woning.Bewoners.Add(
                    new Bewoner { Naam = "Roderick" });
            woning.Bewoners.Add(
                    new Bewoner { Naam = "John" });
            context.SaveChanges();
        }


        private static void GetWoningen(WoningContext context)
        {
            var woningen = context.Woningen.Include(w => w.Bewoners).ToList(); 
            foreach (var woning in woningen)
            {
                foreach (var bewoner in woning.Bewoners)
                {
                    Console.WriteLine($"Bewoner: {bewoner.Naam} in woning: {bewoner.Woning.Naam}");
                }
            }
            
            Console.ReadLine();
        }

      



        private static void InsertWoning(WoningContext context)
        {
            context.Woningen.Add(new Woning { Naam = "HuizeFortune" });
            context.SaveChanges();
        }



    }
}







