using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class Supplies : Cargo, ISupply
    {
        private int _unitSuppliesRemaining { get; set; }
        private int _cargoPointsRemaining { get; set; }
        public override int CargoPoints
        {
            get
            {
                return _cargoPointsRemaining + ((_unitSuppliesRemaining > 0) ? 1 : 0);
            }
            set
            {
                _cargoPointsRemaining = value;
            }
        }
        public SupplyType SupplyType { get; set; }
        public int UnitsSupplyPerPoint { get; set; }
        public int UnitsSupplyRemaining
        {
            get
            {
                return _unitSuppliesRemaining;
            }
            set
            {
                _unitSuppliesRemaining = value;
            }
        }

        [JsonIgnore]
        public bool IsOpen
        {
            get
            {
                return UnitsSupplyRemaining < UnitsSupplyRemaining;
            }
        }

        [JsonIgnore]
        public bool IsExausted
        {
            get
            {
                return _cargoPointsRemaining < 0;
            }
        }

        public void AdjustSupplies(int amount)
        {
            bool subtraction = amount < 0;
            int units = Math.Abs(amount) % UnitsSupplyPerPoint;
            int cargo = Math.Abs(amount) % UnitsSupplyPerPoint;

            if (subtraction)
            {
                if (units < _unitSuppliesRemaining)
                {
                    _cargoPointsRemaining -= cargo;
                }
                else
                {
                    _cargoPointsRemaining -= (cargo + 1);
                }
                _unitSuppliesRemaining = UnitsSupplyPerPoint + _unitSuppliesRemaining - units;
            }
            else
            {
                if (units < (UnitsSupplyPerPoint - _unitSuppliesRemaining))
                {
                    _cargoPointsRemaining += cargo;
                    _unitSuppliesRemaining += units;
                }
                else
                {
                    _cargoPointsRemaining += (cargo + 1);
                    _unitSuppliesRemaining = _unitSuppliesRemaining + units - UnitsSupplyPerPoint;
                }
            }

            if (CargoPoints < 1)
            {
                OnExhuastion();
            }
        }
   
        public void OnExhuastion()
        {
           
        }
    }
}
