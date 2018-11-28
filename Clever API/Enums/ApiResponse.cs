using System; 

namespace Clever_API.Enums
{
    public enum TYPE_LEADERBOARD
    {
        WEEK,
        ALL_TIME
    }

    public enum ACTION_IDS
    {
        WATCHED_GAME = 1,
        JOIN_GAME,
        ANSWER_CORRECT,
        WIN_GAME,
        INVITE_FRIEND,
        COMMUNITY_NOTIFY
    }

    public enum ANSWER
    {
        FIRST,
        SECOND,
        THIRD
    }
}