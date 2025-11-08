
using System;

namespace caching_proxy.core
{
    public static class TTLParser
    {
        public static TimeSpan Parse(string ttl)
        {
            // Validar que el string no esté vacío
            if (string.IsNullOrWhiteSpace(ttl))
            {
                throw new ArgumentException("TTL no puede estar vacío.");
            }

            // Obtener la unidad de tiempo (último carácter)
            char timeUnit = ttl[^1];
            // Obtener la parte numérica (todo excepto el último carácter)
            string numberPart = ttl[..^1];

            // Intentar convertir la parte numérica (string) a entero
            if (!int.TryParse(numberPart, out int timeValue) || timeValue < 0)
            {
                throw new ArgumentException("Formato de TTL inválido.");
            }

            // Devolver el TimeSpan correspondiente según la unidad de tiempo
            return timeUnit switch
            {
                's' => TimeSpan.FromSeconds(timeValue),
                'm' => TimeSpan.FromMinutes(timeValue),
                'h' => TimeSpan.FromHours(timeValue),
                'd' => TimeSpan.FromDays(timeValue),
                _ => throw new ArgumentException("Unidad de tiempo inválida en TTL. Use 's', 'm', 'h' o 'd'."),
            };
        }
    }
}