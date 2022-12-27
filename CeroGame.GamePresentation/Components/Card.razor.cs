using CeroGame.GameService.Enums;
using CeroGame.GameService.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeroGame.GamePresentation.Components
{
    public partial class Card
    {
        [Parameter]
        public CardModel CardModel { get; set; }


    }
}
