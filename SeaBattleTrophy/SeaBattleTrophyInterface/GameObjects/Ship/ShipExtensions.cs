using Maths.Geometry;
using System;

namespace SeaBattleTrophyGame
{
    public static class ShipExtensions
    {

        public static Vector2D CalculateForceOnShip(this IShipReadOnly ship, IWind wind)
        {
            // Calculate water drift force. It is dependent on the water drift (that is based on the wind), and the 
            // size of the ship hull. 
            //const double waterDriftConstant = 1.0;
            //const double shipSizeVariable = 1000;
            const double windForceConstant = 100;
            const double waterFrictionConstant = 20;

            var windVector = Vector2D.DirectionFromAngle(wind.Angle) * wind.Velocity;
            var waterDriftVector = windVector;
            //var waterDriftForce = waterDriftVector * shipSizeVariable * waterDriftConstant;

            // Calculate wind force on the ship. It is dependent on wind, sail level, current velocity 
            // of the ship and current angle of the ship (technically the angle of the sails). 
            // This could later be calculated for each sail separately. Force is based on velocity difference squared.
            var windShipVelocityDifference = windVector - ship.ShipStatus.Velocity;
            var shipDirection = Vector2D.DirectionFromAngle(ship.AngleInDegrees);

            // For now we assume all forces perpendicular to ship direction are cancelled out by water friction. (Since the broad side of the ships
            // is so big)
            var windForceInShipDirection = Math.Pow(shipDirection.Dot(windShipVelocityDifference),2) *
                SailLevelSpeedModifier(ship.ShipStatus.SailLevel) * windForceConstant;
            var windForce = shipDirection * windForceInShipDirection;

            // Calculate water friction force on the ship. (the difference between velocity and
            // the current water drift squared), it's affected by the size of the ship.

            // For now only consider friction along ship direction.
            var waterShipVelocityDifference = waterDriftVector - ship.ShipStatus.Velocity;


            var waterFrictionForce = waterShipVelocityDifference.Normalized() * waterShipVelocityDifference.SquaredLength() * waterFrictionConstant;

            return windForce + waterFrictionForce;
        }

        // This should be moved somewhere
        private static double SailLevelSpeedModifier(SailLevel sailLevel)
        {
            switch (sailLevel)
            {
                case SailLevel.NoSails:
                    return 0.0;
                case SailLevel.LowSails:
                    return 0.4;
                case SailLevel.CombatSails:
                    return 0.6;
                case SailLevel.FullSails:
                    return 0.8;
                case SailLevel.FullSailsWithLeadSail:
                    return 1.0;
                default:
                    throw new ArgumentOutOfRangeException("This sail speed is not supported!");
            }
        }
    }
}
