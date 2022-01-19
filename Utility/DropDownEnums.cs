using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FreebirdTech.Utility
{
    public static class EnumExtensions
    {
        private static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static string GetDisplayNameForEnum(this Enum enumvalue)
        {
            return enumvalue.GetAttribute<DisplayAttribute>().Name;
        }
    }

    public enum ECategoriaProfissional
    {
        [Display(Name = "Serviço")]
        SERVICO,
        [Display(Name = "Aluguel de Equipamento")]
        ALUGUEL,
        [Display(Name = "Cineasta")]
        CINEASTA,
        [Display(Name = "Tecnologia")]
        TECNOLOGIA,
        [Display(Name = "Roteirista")]
        ROTEIRISTA,
        [Display(Name = "Fotógrafo")]
        FOTOGRAFO
    }

    public enum EPortfolioType
    {
        [Display(Name = "Imagem de Capa")]
        IMAGEM_CAPA,
        [Display(Name = "Imagem")]
        IMAGEM,
        [Display(Name = "Embed")]
        EMBED,
        [Display(Name = "Selecione Um tipo de Portfolio")]
        NONE
    }
}
