﻿using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PokemonAPI.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly PokemonAPIContext _context;
        public PokemonapiDAL DAL = new PokemonapiDAL();

        public FavoritesController(PokemonAPIContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Favorites> Favorites = _context.Favorites.Where(x => x.UserId == userId).ToList();
            // // //
            List<Pokemon> favoritePokemon = new List<Pokemon>();

            foreach (Favorites fav in Favorites)
            {
                favoritePokemon.Add(DAL.ConvertToPokemonModelsFav(fav.PokedexNumber));
            }
            // // //
            return View(favoritePokemon);
        }
    

        // Create View Form
        [HttpGet]
        public IActionResult AddToFavorites()
        {
            return View(new Favorites());
        }

        // Create Add
        [HttpPost]
        public IActionResult AddToFavorites(Favorites favorites)
        {
            if (ModelState.IsValid)
            {

                favorites.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                // Add to the database
                _context.Favorites.Add(favorites);

                // Save changes to database
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(AddToFavorites), favorites);
        }
        public IActionResult RemoveFromFavorites(int id)
        {
            Favorites f = _context.Favorites.Find(id);
            return View(f);
        }
        [HttpPost]
        public IActionResult RemoveFromFavorites(Favorites favorites)
        {
            _context.Favorites.Remove(favorites);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
