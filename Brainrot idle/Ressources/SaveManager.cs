using System;
using System.IO;
using System.Text.Json;

namespace Brainrot_idle.Ressources
{
    public static class SaveManager
    {
        private static readonly string SavePath =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "BrainrotIdle",
                "save.json");

        public static void Save()
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(SavePath));

            SaveData data = new()
            {
                Points = GameState.points,
                AuraParSeconde = GameState.auraParSeconde,

                NbAmeliorations = GameState.nbAmeliorations,
                PrixAmeliorations = GameState.prixAmeliorations,

                MeilleurScoreSnake = GameState.MeilleurScoreSnake,
                MeilleurScoreMorpion = GameState.MeilleurScoreMorpion,

                IsSnakeDebloque = GameState.IsSnakeDebloque,
                IsCombatDebloque = GameState.IsCombatDebloque,
                IsMorpionDebloque = GameState.IsMorpionDebloque,

                XpActuelle = GameState.XpActuelle,
                XpRequise = GameState.XpRequise,
                Niveau = GameState.Niveau,
                PointsDeCompetence = GameState.PointsDeCompetence,

                NiveauApprenti = GameState.NiveauApprenti,
                MultiplicateurXp = GameState.MultiplicateurXp,

                NiveauCliqueur = GameState.NiveauCliqueur,
                MultiplicateurAuraParClic =
                    GameState.MultiplicateurAuraParClic,

                AuraBonus = GameState.AuraBonus,
                AuraBonusFlat = GameState.AuraBonusFlat,

                MusiquesActives =
                    GameState.MusiquesActives.ToList()
            };

            string json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(SavePath, json);
        }

        public static void Load()
        {
            if (!File.Exists(SavePath))
                return;

            string json = File.ReadAllText(SavePath);

            SaveData data =
                JsonSerializer.Deserialize<SaveData>(json);

            if (data == null)
                return;

            GameState.points = data.Points;
            GameState.auraParSeconde = data.AuraParSeconde;

            GameState.nbAmeliorations = data.NbAmeliorations;
            GameState.prixAmeliorations = data.PrixAmeliorations;

            GameState.MeilleurScoreSnake =
                data.MeilleurScoreSnake;

            GameState.MeilleurScoreMorpion =
                data.MeilleurScoreMorpion;

            GameState.IsSnakeDebloque =
                data.IsSnakeDebloque;

            GameState.IsCombatDebloque =
                data.IsCombatDebloque;

            GameState.IsMorpionDebloque =
                data.IsMorpionDebloque;

            GameState.XpActuelle = data.XpActuelle;
            GameState.XpRequise = data.XpRequise;
            GameState.Niveau = data.Niveau;
            GameState.PointsDeCompetence =
                data.PointsDeCompetence;

            GameState.NiveauApprenti =
                data.NiveauApprenti;

            GameState.MultiplicateurXp =
                data.MultiplicateurXp;

            GameState.NiveauCliqueur =
                data.NiveauCliqueur;

            GameState.MultiplicateurAuraParClic =
                data.MultiplicateurAuraParClic;

            GameState.AuraBonus =
                data.AuraBonus;

            GameState.AuraBonusFlat =
                data.AuraBonusFlat;

            GameState.MusiquesActives =
                new HashSet<string>(
                    data.MusiquesActives ?? new());
        }
    }
}