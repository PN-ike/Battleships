using System;

namespace BattleshipsServer
{
    public sealed class Message
    {
        
        public const String IP = "10.0.0.3";

        public const String SET_YOUR_FLEET = "SetFleet";
        public const String WAIT = "Wait";
        public const String START = "Start";
        public const String ACK = "ok";
        public const String CYA = "c ya";
        public const String SHOOT = "shoot";
        public const String RECEIVE_COORDINATES = "receiveCoordinates";
        public const String NEW_DAMAGE = "damage";
        public const String MISSED = "missed";
        public const String YOU_HAVE_WON = "youwin";
        public const String CHECK_DAMAGE = "checkDamage";

    }
}
