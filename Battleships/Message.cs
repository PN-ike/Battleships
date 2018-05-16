using System;

namespace BattleshipsServer
{
    public sealed class Message
    {
        public const String IP = "10.0.0.3";

        public const String SET_YOUR_FLEET = "setFleet";
        public const String WAIT = "wait";
        public const String START = "start";
        public const String ACK = "ok";
        public const String CYA = "cya";
        public const String SHOOT = "shoot";
        public const String RECEIVE_COORDINATES = "recCoords";
        public const String NEW_DAMAGE = "dmg";
        public const String MISSED = "miss";
        public const String YOU_HAVE_WON = "youWin";
        public const String CHECK_DAMAGE = "checkDmg";
        public const String WAIT_FOR_PLAYER_2 = "waitP2";
    }
}
