using Microsoft.EntityFrameworkCore;
using VideoGameCharacterApi.Data;
using VideoGameCharacterApi.Models;
using VideoGameCharacterApi.Dtos;

namespace VideoGameCharacterApi.Services
{
    public class VideoGameCharacterService(AppDbContext context) : IVideoGameCharacterService
    {
        public async Task<CharacterResponse> AddCharacterAsync(CreateCharacterRequest character)
        {
            var newCharacter = new Character
            {
                Name = character.Name,
                Game = character.Game,
                Role = character.Role,
            };

            context.Characters.Add(newCharacter);
            await context.SaveChangesAsync();

            return new CharacterResponse
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Game = newCharacter.Game,
                Role = newCharacter.Role,
            };
        }

        public async Task<bool> DeleteCharacterAsync(int id)
        {
            var characterToDelete = await context.Characters.FindAsync(id);
            if(characterToDelete is null)
                return false;

            context.Characters.Remove(characterToDelete);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CharacterResponse>> GetAllCharactersAsync()
            => await context.Characters.Select(c => new CharacterResponse
            {
                Id = c.Id,
                Name = c.Name,
                Game = c.Game,
                Role = c.Role,
            }).ToListAsync();

        public async Task<CharacterResponse?> GetCharacterByIdAsync(int id)
        {
            var result = await context.Characters
                .Where(c => c.Id == id)
                .Select(c => new CharacterResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role,
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> UpdateCharacterAsync(int id, UpdateCharacterRequest character)
        {
            var existingCharater = await context.Characters.FindAsync(id);
            if (existingCharater is null)
                return false;

            existingCharater.Name = character.Name;
            existingCharater.Game = character.Game;
            existingCharater.Role = character.Role;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
