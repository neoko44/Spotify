﻿using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class TrackDto:IDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ArtistDto> Artists { get; set; }
        public AlbumDto Album { get; set; }
        public string Duration { get; set; }

    }
}
