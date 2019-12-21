using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    // class that responsible for repeted messages in the Console Ui.
    public class UIMessages
    {
        
        private const string k_MainMenuMsg = @"          
Select the operation you wish to do:
1. Add a new School to the program.
2. Add a new Entity.
3. Get a Cached Entity.
4. Update a Cached Entity.
5. Remove a Cached Entity.
6. Exit
";

        private const string k_SeperatorMsg = "--------------------------------------------------";

        private const string k_OperationSuccessfulMsg = "Operation successfully completed!";
        private const string k_OperationFailedMsg = "Operation failed!";
        private const string k_SchoolNotFoundMsg = "School does not exist";
        private const string k_SchoolEnterEntityIdMsg = "Please enter Entity id: ";
        private const string k_EnterSchoolIdMsg = "Please enter School id: ";
        private const string k_PressAnyKeyToContinueMsg = "press any key to continue...";
        private const string k_InvalidInputMsg = "Invalid input, please try again.";
        private const string k_InvalidSelectionMsg = "Invalid selection, please try again.";

        public enum eGeneralMessages
        {
            MainMenu,
            OperationSuccess,
            OperationFailedMsg,
            SchoolNotFoundMsg,
            SchoolEnterEntityIdMsg,
            EnterSchoolIdMsg,
            PressAnyKeyToContinue,
            InvalidInput,
            InvalidSelection,
            Seperator,
        }

        public static void DisplayMessages(eGeneralMessages i_MessageToPrint)
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch (i_MessageToPrint)
            {
                case eGeneralMessages.MainMenu:
                    Console.WriteLine(k_MainMenuMsg);
                    break;
                case eGeneralMessages.OperationSuccess:
                    Console.WriteLine(k_OperationSuccessfulMsg);
                    break;
                case eGeneralMessages.OperationFailedMsg:
                    Console.WriteLine(k_OperationFailedMsg);
                    break;
                case eGeneralMessages.SchoolNotFoundMsg:
                    Console.WriteLine(k_SchoolNotFoundMsg);
                    break;
                case eGeneralMessages.SchoolEnterEntityIdMsg:
                    Console.WriteLine(k_SchoolEnterEntityIdMsg);
                    break;
                case eGeneralMessages.EnterSchoolIdMsg:
                    Console.WriteLine(k_EnterSchoolIdMsg);
                    break;
                case eGeneralMessages.PressAnyKeyToContinue:
                    Console.WriteLine(k_PressAnyKeyToContinueMsg);
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case eGeneralMessages.InvalidInput:
                    Console.WriteLine(k_InvalidInputMsg);
                    break;
                case eGeneralMessages.InvalidSelection:
                    Console.WriteLine(k_InvalidSelectionMsg);
                    break;
                case eGeneralMessages.Seperator:
                    Console.WriteLine(k_SeperatorMsg);
                    break;
                default:
                    break;
            }
        }
    }
}

