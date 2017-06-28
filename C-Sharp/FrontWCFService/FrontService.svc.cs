﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.SqlServer;
using FrontWcfService;
using FrontWcfService.App_Code;

namespace FrontWcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class FrontService : IDecryptageService
    {
        public State GetState(string username, string token)
        {
            State state = new State();

            if (AuthToken(username,token))
            {
                ModelUser userSystem = new ModelUser();

                switch (userSystem.getProgressUser(username))
                {
                    case 0:
                        state.amount = 0;
                        state.comment = "Ready to bruteforce";
                        break;
                    case 1:
                        state.amount = 1;
                        state.comment = "25% progress";
                        break;
                    case 2:
                        state.amount = 2;
                        state.comment = "50% progress";
                        break;
                    case 3:
                        state.amount = 3;
                        state.comment = "75% progress";
                        break;
                    case 4:
                        state.amount = 4;
                        state.comment = "100% Finished";
                        break;
                    default:

                        break;

                }
                
                return state;
            }
            else
            {
                state.amount = 0; // 0 ready - 1 25% key generated - 2 50% key generated - 3 75% key generated - 4 100% key generated
                state.comment = "Invalid token";
                return state;
            }

        }

        public bool LaunchDecryptProcess(string[] str, string token)
        {
            throw new NotImplementedException();
        }

        public LogInfo Login(LogInfo loginInfo)
        {
            ModelUser userSystem = new ModelUser();

            if (userSystem.isUserExist(loginInfo.username, loginInfo.password))
            {
                string token = TokenGen.TokenGenerator.GetInstance.BuildSecureToken(30);
                int idUser = userSystem.getIdUser(loginInfo.username);

                userSystem.updateUserToken(idUser, token);

                loginInfo.token = token;
                return loginInfo;
            }
            else
            {
                loginInfo.token = "";
                return loginInfo;
            }

        }

        private bool AuthToken(string username ,string token)
        {
            ModelUser userSystem = new ModelUser();
            if (userSystem.isTokenExist(username, token))
                return true;
            else
                return false;
        }
    }
}
