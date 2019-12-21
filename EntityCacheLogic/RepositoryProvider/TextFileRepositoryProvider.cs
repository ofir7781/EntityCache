using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EntityCache
{
    public class TextFileRepositoryProvider<T> : IRepositoryProvider<T> where T : Entity, new()
    {
        private readonly object m_LockObject = new object();
        private static List<string> m_DocumentFileNames = new List<string>(); // Static list that will save all the names of the text file throughout the entire program.
        private string m_RepositoryName;
       
        public TextFileRepositoryProvider(string i_RepositoryName)
        {
            lock(m_LockObject)
            {
                if (!m_DocumentFileNames.Contains(i_RepositoryName))
                {
                    m_DocumentFileNames.Add(i_RepositoryName);
                    m_RepositoryName = i_RepositoryName;
                }
                else
                {
                    throw new Exception("File name is already exist");
                }
            }
        }

        public void AddNewEntityToRepository(int i_EntityId, T i_Entity)
        {
            Dictionary<string, object> entityDictionary = ObjectAndDictionaryTwoWayConverter.ConvertObjectToDictionary(i_Entity);
            FileStream fs;

            lock (m_LockObject)
            {
               
                if (File.Exists(m_RepositoryName))
                {
                    fs = new FileStream(m_RepositoryName,
                                        FileMode.Append,
                                        FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(m_RepositoryName,
                                        FileMode.Create,
                                        FileAccess.Write);
                }

                StreamWriter writerToTextFile = new StreamWriter(fs);
                try
                {
                    // Adding every Entity in a permanent format.
                    foreach (KeyValuePair<string, object> entry in entityDictionary)
                    {

                        writerToTextFile.Write(entry.Key);
                        writerToTextFile.Write(" : ");
                        writerToTextFile.Write(entry.Value);
                        writerToTextFile.Write("  ");
                    }

                    writerToTextFile.WriteLine("");

                    fs.Flush();
                    writerToTextFile.Flush();
                    writerToTextFile.Close();
                }
                finally
                {
                    writerToTextFile.Close();
                }           
            }
        }


        public bool RemoveEntityFromRepository(int i_EntityId)
        {
            bool actionSucceed = false;
            FileStream fs;
            StringBuilder readAllLine = new StringBuilder();

            lock(m_LockObject)
            {
                if (File.Exists(m_RepositoryName))
                {
                    fs = new FileStream(m_RepositoryName,
                                        FileMode.Open,
                                        FileAccess.Read);
                }
                else
                {
                    throw new Exception("File not exist");
                }

                StreamReader readerFromText = new StreamReader(fs);
                try
                {
                    // Read lines of text file and add to a new stringBuilder. only the line we want to remove 
                    // dont add to the new stringBuilder.
                    while (readerFromText.EndOfStream == false)
                    {
                        string rowStr = readerFromText.ReadLine();
                        if (!rowStr.Contains(i_EntityId.ToString()))
                        {
                            readAllLine.AppendLine(rowStr);
                        }
                    }

                    actionSucceed = true;
                    fs.Flush();
                    readerFromText.Close();
                }
                finally
                {
                    readerFromText.Close();
                }

                
                fs = new FileStream(m_RepositoryName,
                                        FileMode.Truncate,
                                        FileAccess.Write);
                StreamWriter writerToTextFile = new StreamWriter(fs);

                try
                {
                    //Add the new stringBuilder to the file after i cleaned it.
                    writerToTextFile.Write(readAllLine);

                    fs.Flush();
                    writerToTextFile.Flush();
                    writerToTextFile.Close();
                }
                finally
                {
                    writerToTextFile.Close();
                }
               
            }

            return actionSucceed;
        }

        public bool UpdateEntityInRepository(int i_EntityId, T i_Entity)
        {
            bool actionSucceed = false;
            FileStream fs;
            StringBuilder readAllLine = new StringBuilder();
            Dictionary<string, object> entityDictionary = ObjectAndDictionaryTwoWayConverter.ConvertObjectToDictionary(i_Entity);

            lock(m_LockObject)
            {
                if (File.Exists(m_RepositoryName))
                {
                    fs = new FileStream(m_RepositoryName,
                                        FileMode.Open,
                                        FileAccess.Read);
                }
                else
                {
                    throw new Exception("File not exist");
                }

                StreamReader readerFromText = new StreamReader(fs);

                try
                {
                    // Read lines of text file and add to a new stringBuilder. only the line we want to update 
                    // dont add to the new stringBuilder.
                    while (readerFromText.EndOfStream == false)
                    {
                        string rowStr = readerFromText.ReadLine();
                        if (!rowStr.Contains(i_EntityId.ToString()))
                        {
                            readAllLine.AppendLine(rowStr);
                        }
                        else
                        {
                            // The line we want to change we will do it here according to new info the method got.
                            foreach (KeyValuePair<string, object> entry in entityDictionary)
                            {

                                readAllLine.Append(entry.Key);
                                readAllLine.Append(" : ");
                                readAllLine.Append(entry.Value);
                                readAllLine.Append("  ");
                            }
                            readAllLine.AppendLine();
                        }
                    }

                    fs.Flush();
                    readerFromText.Close();
                }
                finally
                {
                    readerFromText.Close();
                }
                

                fs = new FileStream(m_RepositoryName,
                                        FileMode.Truncate,
                                        FileAccess.Write);

                StreamWriter writerToTextFile = new StreamWriter(fs);
                try
                {
                    writerToTextFile.Write(readAllLine);

                    fs.Flush();
                    writerToTextFile.Flush();
                    writerToTextFile.Close();
                    actionSucceed = true;
                }
                finally
                {
                    writerToTextFile.Close();
                }
                
            }
        
            return actionSucceed;
        }
        public Dictionary<int, T> ReadAllExistingEntityFromRepository()
        {
            T entityObj;
            Dictionary<int, T> existingEntityFromRepository = new Dictionary<int, T>();       
            FileStream fs;
          
            lock(m_LockObject)
            {
                if (File.Exists(m_RepositoryName))
                {
                    fs = new FileStream(m_RepositoryName,
                                        FileMode.Open,
                                        FileAccess.Read);
                }
                else
                {
                    throw new Exception("File not exist");
                }

                StreamReader readerFromText = new StreamReader(fs);
                try
                {
                    while (readerFromText.EndOfStream == false)
                    {
                        // Read all lines from text file, convert them to Dictionary<string, object> 
                        // and let the ststic class convert to our desired object.
                        string rowStr = readerFromText.ReadLine();
                        Dictionary<string, object> singleEntityfromRepository;
                        singleEntityfromRepository = converStringToDictionary(rowStr);
                        entityObj = ObjectAndDictionaryTwoWayConverter.ConvertDictionaryToObject<T>(singleEntityfromRepository);
                        existingEntityFromRepository.Add(entityObj.getId(), entityObj);
                    }

                    fs.Flush();
                    readerFromText.Close();
                }
                finally
                {
                    readerFromText.Close();
                }
                
            }

            return existingEntityFromRepository;
        }

        //This function convert any string to a Dictionary<string, object>
        private Dictionary<string, object> converStringToDictionary(string i_RowStr)
        {
            Dictionary<string, object> singleEntityfromRepository = new Dictionary<string, object>();
            StringBuilder readStringKey = new StringBuilder();
            string stringKey = String.Empty;
            StringBuilder readObjectValue = new StringBuilder();
            string stringVal = String.Empty;

            int updateIndex = 0;
            int flagKeyOrValue = 0; // 0 means the string key value and 1 is the object value

            for (int i = 0; i < i_RowStr.Length; i++)
            {
                if (!i_RowStr[i].Equals(' ') && flagKeyOrValue == 0)
                {
                    for (int j = i; j < i_RowStr.Length; j++)
                    {
                        if (i_RowStr[j].Equals(' '))
                        {
                            readStringKey.Append(i_RowStr, i, j - i);
                            stringKey = readStringKey.ToString();
                            updateIndex = j;
                            flagKeyOrValue = 1;
                            break;
                        }
                    }

                    i = updateIndex;
                }

                if (!i_RowStr[i].Equals(' ') && flagKeyOrValue == 1)
                {
                    if (!i_RowStr[i].Equals(':'))
                    {
                        for (int j = i; j < i_RowStr.Length; j++)
                        {
                            if (i_RowStr[j].Equals(' '))
                            {
                                readObjectValue.Append(i_RowStr, i, j - i);                    
                                stringVal = readObjectValue.ToString();
                                updateIndex = j;
                                flagKeyOrValue = 0;
                                break;
                            }
                        }
                        i = updateIndex;
                        singleEntityfromRepository.Add(stringKey, stringVal);
                        readStringKey.Clear();
                        stringKey = String.Empty;
                        readObjectValue.Clear();
                        stringVal = String.Empty;
                    }
                }
            }
            return singleEntityfromRepository;
        }

    }
}