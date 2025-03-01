using CriticalCommonLib.Interfaces;
using Lumina;
using Lumina.Data;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

namespace CriticalCommonLib.Sheets
{
    public class LevelEx : Level, ILocation
    {
        public override void PopulateData(RowParser parser, GameData gameData, Language language)
        {
            base.PopulateData(parser, gameData, language);
            MapEx = new LazyRow< MapEx >( gameData, Map.Row, language );
            PlaceName = Map.Value!.PlaceName;

        }
        
        public LazyRow< MapEx > MapEx { get; set; }


        public string FormattedName
        {
            get
            {
                var map = Map.Value?.PlaceName.Value?.Name.ToString() ?? "Unknown Map";
                var territory =  Territory.Value?.PlaceNameRegion.Value?.Name.ToString() ?? "Unknown Territory";
                return map + " - " + territory;
            }
        }

        /// <summary>
        ///     Gets the X-coordinate on the 2D-map.
        /// </summary>
        /// <value>The X-coordinate on the 2D-map.</value>
        public double MapX 
        {
            get
            {
                if (MapEx.Value != null)
                {
                    return MapEx.Value.ToMapCoordinate3d(X, MapEx.Value.OffsetX);
                }

                return 0;
            }
        }

        /// <summary>
        ///     Gets the Y-coordinate on the 2D-map.
        /// </summary>
        /// <value>The Y-coordinate on the 2D-map.</value>
        public double MapY
        {
            get
            {
                if (MapEx.Value != null)
                {
                    return MapEx.Value.ToMapCoordinate3d(Z, MapEx.Value.OffsetY);
                }

                return 0;
            }
        }

        public override string ToString()
        {
            return FormattedName;
        }

        public LazyRow<PlaceName> PlaceName { get; set; }
    }
}