using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    //This class works with the console and check for valid input from the user.
    public  class EntityCacheConsoleUI<T> where T : Entity, new() 
    {
        private  readonly SchoolsManager<T> r_SchoolsManager;
        private  eSystemStatus m_SystemStatus;

        private enum eEntityCacheOperations
        {
            AddNewSchool = 1,
            AddNewEntity = 2,
            GetCachedEntity = 3,
            UpdateCachedEntity = 4,
            RemoveCachedEntity = 5,
            Exit = 6,
        }

        private enum eSystemStatus
        {
            Off = 0,
            On = 1,
        }

        public EntityCacheConsoleUI()
        {
            r_SchoolsManager = new SchoolsManager<T>();
            Console.ForegroundColor = ConsoleColor.White;
            m_SystemStatus = eSystemStatus.On;
        }

        public void Run()
        {
            while (m_SystemStatus == eSystemStatus.On)
            {
                printMainMenu();
                makeGarageOperations();
            }
        }

        private void printMainMenu()
        {
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.Seperator);
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.MainMenu);
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.Seperator);
            for (int row = 1; row <= 10; row++)
            {
                Console.SetCursorPosition(49, row);
                Console.Write('|');
            }

            Console.SetCursorPosition(0, 11);
        }

        private void makeGarageOperations()
        {
            bool isValidChoice = false;
            while (isValidChoice == false)
            {
                int operation = readIntFromConsole();
                try
                {
                    preformOperations(operation);
                    isValidChoice = true;
                }
                catch (ArgumentException)
                {
                    UIMessages.DisplayMessages(UIMessages.eGeneralMessages.InvalidSelection);
                }
            }
        }

        // Each operation of the menu in the console has its own function method.
        private void preformOperations(int i_OperationChoice)
        {
            bool isOperationSuccessful = false;
            switch ((eEntityCacheOperations)i_OperationChoice)
            {
                case eEntityCacheOperations.AddNewSchool:
                    isOperationSuccessful = addNewSchoolToDictionary();
                    break;
                case eEntityCacheOperations.AddNewEntity:
                    isOperationSuccessful = addNewEntityTosSpecificSchool();
                    break;
                case eEntityCacheOperations.GetCachedEntity:
                    getCachedEntity();
                    break;
                case eEntityCacheOperations.UpdateCachedEntity:
                    isOperationSuccessful = updateCachedEntity();
                    break;
                case eEntityCacheOperations.RemoveCachedEntity:
                    isOperationSuccessful = removeCachedEntity();
                    break;
                case eEntityCacheOperations.Exit:
                    m_SystemStatus = eSystemStatus.Off;
                    break;
                default:
                    throw new ArgumentException();
            }

            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.Seperator);
            if (isOperationSuccessful)
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.OperationSuccess);
            }

            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.PressAnyKeyToContinue);
        }


        private bool addNewSchoolToDictionary()
        {
            bool isSchoolExists = false;
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.EnterSchoolIdMsg);
            int schoolId = readIntFromConsole();
            isSchoolExists = r_SchoolsManager.IsSchoolExists(schoolId);
            if (isSchoolExists == false)
            {
                try
                {
                    createNewSchool(schoolId);
                    isSchoolExists = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isSchoolExists = false;
                }

            }
            else
            {
                Console.WriteLine("School already exist");
                isSchoolExists = false;
            }
            return isSchoolExists;
        }

        private void createNewSchool(int i_SchoolId)
        {
            School<T> newSchool; 
            Console.WriteLine("Select School InitializeState:");
            string[] schoolInitializeState = Enum.GetNames(typeof(EntityCache<T>.eInitializeState));
            for (int i = 1; i <= schoolInitializeState.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i, schoolInitializeState[i - 1]);
            }

            int InitializeState = readIntFromConsole();
            InitializeState--;
            Console.WriteLine("Choose text file name to create:");
            string fileName = readNonEmptyStringFromConsole();

            newSchool = new School<T>(i_SchoolId, fileName, (EntityCache<T>.eInitializeState)InitializeState);
            r_SchoolsManager.Schools.Add(newSchool.id, newSchool);                     
        }


        private bool updateCachedEntity()
        {
            Entity entity;
            bool isSpecificSchoolExists = false;
            bool checkForSameKey = true;
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.EnterSchoolIdMsg);
            int SchoolId = readIntFromConsole();
            isSpecificSchoolExists = r_SchoolsManager.IsSchoolExists(SchoolId);
            if (isSpecificSchoolExists == true)
            {
                try
                {
                    School<T> school = r_SchoolsManager.GetRequestedSchool(SchoolId);
                    entity = createNewEntity(checkForSameKey, school);                 
                    if (entity is T)
                    {
                        school.UpdateCachedEntity(entity.getId(), (T)entity);
                    }
                    else
                    {
                        UIMessages.DisplayMessages(UIMessages.eGeneralMessages.OperationFailedMsg);
                        isSpecificSchoolExists = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isSpecificSchoolExists = false;
                }

            }
            else
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolNotFoundMsg);
            }
            return isSpecificSchoolExists;
        }
            

        private bool removeCachedEntity()
        {
            bool isSpecificShoolExists = false;
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.EnterSchoolIdMsg);
            int schoolId = readIntFromConsole();
            isSpecificShoolExists = r_SchoolsManager.IsSchoolExists(schoolId);
            if (isSpecificShoolExists == true)
            {
                try
                {
                    School<T> school = r_SchoolsManager.GetRequestedSchool(schoolId);
                    UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolEnterEntityIdMsg);
                    int entityId = readIntFromConsole();
                    school.RemoveCachedEntity(entityId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isSpecificShoolExists = false;
                }

            }
            else
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolNotFoundMsg);
            }
            return isSpecificShoolExists;
        }

        private bool addNewEntityTosSpecificSchool()
        {
            Entity entity;
            bool isSpecificSchoolExists = false;
            bool checkForSameKey = false; // This will help us in method createNewEntity if we need same or different keys according to the action we commit.
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.EnterSchoolIdMsg);
            int schoolId = readIntFromConsole();
            isSpecificSchoolExists = r_SchoolsManager.IsSchoolExists(schoolId);
            if (isSpecificSchoolExists == true)
            {
                try
                {
                    School<T> school = r_SchoolsManager.GetRequestedSchool(schoolId);
                    entity = createNewEntity(checkForSameKey, school);
                    if(entity is T)
                    {                      
                        school.AddNewEntity(entity.getId(), (T)entity);
                    }
                    else
                    {
                        Console.WriteLine("Operation failed!");
                        isSpecificSchoolExists = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isSpecificSchoolExists = false;
                }

            }
            else
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolNotFoundMsg);
                
            }
            return isSpecificSchoolExists;
        }

        private Entity createNewEntity(bool i_CheckForSameKey, School<T> i_School)
        {
            Entity entity;
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolEnterEntityIdMsg);
            int entityId = readIntFromConsole();
            if(i_CheckForSameKey == true)
            {
                if (!i_School.SchoolEntityCache.entityCache.ContainsKey(entityId))
                {
                    throw new Exception("This Entity is not in this school");
                }
            }
            else //i_CheckForSameKey == false
            {
                if (i_School.SchoolEntityCache.entityCache.ContainsKey(entityId))
                {
                    throw new Exception("This Entity is already in this school");
                }
            }
            Console.WriteLine("Please enter Entity age: ");
            int entityAge = readIntFromConsole();
            Console.WriteLine("Please enter Entity name: ");
            string entityName = readNonEmptyStringFromConsole();

            entity = new Student(entityId, entityAge, entityName);
            
            return entity;
        }


        private void getCachedEntity()
        {
            T cachedEntity;
            bool isCachedEntityExists = false;
            UIMessages.DisplayMessages(UIMessages.eGeneralMessages.EnterSchoolIdMsg);
            int schoolId = readIntFromConsole();
            isCachedEntityExists = r_SchoolsManager.IsSchoolExists(schoolId);
            if (isCachedEntityExists == true)
            {
                try
                {
                    School<T> school = r_SchoolsManager.GetRequestedSchool(schoolId);
                    UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolEnterEntityIdMsg);
                    int entityId = readIntFromConsole();
                    cachedEntity = school.GetCachedEntity(entityId);
                    Console.WriteLine("Got cached entity id number: {0} successfully!", cachedEntity.getId());

                }
                catch (Exception)
                {
                    Console.WriteLine("The Entity doesnt exist in the school.");
                    isCachedEntityExists = false;
                }
                
            }
            else
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.SchoolNotFoundMsg);
            }
        }

        private string readNonEmptyStringFromConsole()
        {
            string userInput = Console.ReadLine();
            while (userInput.Length == 0)
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.InvalidInput);
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        private int readIntFromConsole()
        {
            int parsedNumber;
            string userInput = Console.ReadLine();
            while (!int.TryParse(userInput, out parsedNumber))
            {
                UIMessages.DisplayMessages(UIMessages.eGeneralMessages.InvalidInput);
                userInput = Console.ReadLine();
            }

            return parsedNumber;
        }

    }
}
