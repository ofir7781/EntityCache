using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    public class Student : Entity
    {

        private int m_StudentId;
        private int m_Age;
        private string m_Name;


        public Student(int i_StudentId, int i_Age, string i_Name)
        {
            m_StudentId = i_StudentId;
            m_Age = i_Age;
            m_Name = i_Name;
        }

        public Student() { }

        public int StudentId
        {
            get { return getId(); }
            set { setStudentId(value); }
        }

        public int Age
        {
            get { return m_Age; }
            set { m_Age = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public int getId()
        {
            return m_StudentId;
        }

        private void setStudentId(int i_StudentId)
        {
            if (m_StudentId == 0)
            {
                m_StudentId = i_StudentId;
            }
            else
            {
                throw new Exception("Student id was already modified and cannot be changed.");
            }
        }

    }
}
