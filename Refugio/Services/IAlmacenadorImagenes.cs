﻿namespace Refugio.Services
{
    public interface IAlmacenadorImagenes
    {
        Task<string> GuardarImagen(string imgBase64, string container);
    }
}
