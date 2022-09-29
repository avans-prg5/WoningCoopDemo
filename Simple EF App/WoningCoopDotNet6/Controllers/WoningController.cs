using Company.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace WoningCoopDotNet6.Controllers
{

    /*
     * WoningController is nu automatisch de startpagina
     * Dit is aangepast in de Startup.cs zie regel 49 Ipv Woning stond daar eerst Home
     */
    public class WoningController : Controller
    {
        WoningRepo _repo;
        List<Models.Woning> _woningen;
        public WoningController()
       {
            _repo = new WoningRepo();
            _woningen = new List<Models.Woning>();
            var woningen = _repo.GetAll();
            GetBewonersAndWoningen(woningen);

        }
        // GET: WoningController

        public ActionResult Index() 
        {
            return View(_woningen);
        }
        private void GetBewonersAndWoningen(List<Woning> woningen)
        {
            foreach (var item in woningen)
            {
                var wn = new Models.Woning();
                wn.Naam = item.Naam;
                wn.Id = item.Id;
                item.Bewoners.ForEach(b => wn.Bewoners
                .Add(new Models.Bewoner()
                {
                    Naam = b.Naam
                }));
                _woningen.Add(wn);
            }
        }

        private List<Models.Bewoner> GetBewoners(int wnId)
        {
            //_repo etc....
            return null;
        }

        
        public ActionResult Details(int id)
        {
            var wn = _woningen.FirstOrDefault(w => w.Id == id);
            return View(wn);
        }

        // GET: WoningController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WoningController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Woning woning)
        {
            
            try
            {
                /*
                 * Check hier de repo en zie wat ik doe met het toevoegen van de woning
                 * Denk aan het disconnected recordset verhaal
                 */ 
              //  _repo.AddWoning(new Woning() { Naam = woning.Naam });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateBewoner()
        {
            //var addBewoner = new Models.AddBewonerViewModel();
            //_woningen.ForEach(w => addBewoner.Woningen.Add(
            //    new SelectListItem() { Value = w.Id.ToString(),Text = w.Naam}
            //    ));

            //################ Advanced ######################################
            /* 
             * Hieronder staat een andere oplossing. Wat ik hier doe is _woningen in de ViewModel stoppen
             * en laat het creeren van een lijst over aan die klasse. Hij weet tenslotte 
             * hoe de View a.h.w. eruit moet komen te zien. Daarmee haal ik een stuk verantwoordelijkheid 
             * weg bij de controller. Het enige wat de controller nu weer doet is verkeersregelaar spelen, nl.
             * 1) zorgen dat de woningen en zijn bewoners worden opgehaald uit de DB daar een lijst van maken (_woningen)
             * en vervolgens deze doorgeven aan de CreateSelectionList van de viewmodel.
             * 
             * En niet onbelangrijk!..... We bereiken nu een veel leesbaardere code, zowel hier in de controller 
             * als in de CreateSelectionList function
             */
            var addBewoner = new Models.AddBewonerViewModel();
            addBewoner.CreateSelectionList(_woningen);
            return View(addBewoner);
        }

        [HttpPost]
        public ActionResult CreateBewoner(Models.AddBewonerViewModel bewoner)
        {

            try
            {
                /*
                 * Hieronder is de ModelState property gebruikt
                 * die, in dit geval de Required attribuut op de property Naam van de AddBewonerViewModel
                 * checkt. Experimenteer even voor jezelf met de 'All' en 'ModelOnly' in de CreateBewoner View
                 */
                if (ModelState.IsValid)
                {
                    //_repo.AddBewonerToWoning(bewoner.GetWoningId(), bewoner.Naam);
                    return RedirectToAction(nameof(Index));
                }
                return View();   
            }
            catch
            {
                return View();
            }
        }

        /*
         * Probeer zelf eens een edit en de delete eens te maken
         */
        
    }
}
