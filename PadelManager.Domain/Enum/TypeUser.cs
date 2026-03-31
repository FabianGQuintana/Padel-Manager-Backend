using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Enum
{
    public enum TypeUser
    {
        Admin = 0,        //MODO DIOS / DEVELOPER
        Organizador = 1,  //MANAGER
        Invitado = 2,    //GUEST USER
        Tanteador = 3,  //SCORER
        Jugador = 4     //PLAYER
    }
}
