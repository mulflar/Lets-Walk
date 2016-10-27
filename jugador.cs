using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy;

namespace Perro
{
    internal static class jugador
    {
        public static void añadirjugador()
        {
            ServiceManager.Client.LoginAsync((user1, s2) =>
                {
                    user1.GamePlayers.AddAsync((result, s1) =>
                        {
                        }, name:"Name", board:"perro", rank:null, latitude:0.0, longitude: 0.0, appTag: "perro", state: null);
                }, token: ServiceManager.User.Token, state:null);
        }
        public static void añadirpuntuacion(int x,string board)
        {
            ServiceManager.Client.LoginAsync((user, state) =>
                {
                    user.GameScores.AddAsync((result, s1) =>
                        {
                        },score:x,board:board, rank:"",latitude:0,longitude:0,oneScorePerPlayer:false,appTag:"perro", state:null);
                },username:ServiceManager.User.Name, password:MainPage.password,state:null);



        }
    }
}
