﻿using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOS
{
    public class ReadCinemaDto
    {
      
        public int Id { get; set; }
        public string Nome { get; set; }

        public ReadEnderecoDto Endereco { get; set; }

        public ICollection<ReadSessaoDto> Sessoes { get; set; }


    }
}
