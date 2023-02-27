using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project
{
	class Congratulator
	{

        enum Comands{
            All=1, 
            New,
            Change,
            Remove,
            Nearest,
            Exit
        }

		static void Main(string[] args)
		{   

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            const string fileName = "data.txt";

            Birthdays b = new Birthdays();
            
            
            //string str = "Даня 05.02\nАлина 26.03\nАндрей 07.10\nДима 03.10\nНикита 08.11\nГаля 10.10\nПолина 09.03\nВаня 27.02\nМиша 29.03";
            //File.WriteAllText("data.txt", str);

            bool isFileRead = b.ReadFromFile(fileName);
            if(isFileRead == false){
                Console.WriteLine("Ошибка при чтении файла!");
                Console.WriteLine("Будет создан новый файл\n");
            }            
            

                Console.WriteLine("Список доступных функций: \n");
                Console.WriteLine("1. - Весь список");
                Console.WriteLine("2. - Добавить запись");
                Console.WriteLine("3. - Изменить запись");
                Console.WriteLine("4. - Удалить запись");
                Console.WriteLine("5. - Список сегодняшних и ближайших дней рождения");
                Console.WriteLine("6. - Выход");
                Console.WriteLine();

            b.PrintNear();

                int argument = (int)Comands.Nearest;
                bool isArgumentCorrect;
                string input = "";

                while(argument != (int)Comands.Exit)
                {
                    isArgumentCorrect = true;
                    input = Console.ReadLine();
                    if(IsArgumentCorrect(input))
                        argument = Convert.ToInt32(input);
                    else
                        isArgumentCorrect = false;

                    
                    //Console.WriteLine("Была введена команда: \"{0}\"\n", argument);

                    if(isArgumentCorrect){
                        switch(argument){
                            case (int)Comands.All:
                            
                                b.PrintAll();
                            
                                break;

                            case (int)Comands.New:
                                Console.Write("Введите имя: ");
                                string name = Console.ReadLine();
                                
                                Console.Write("Введите дату в формате [dd.mm]: ");
                                string date = Console.ReadLine();
                            
                                bool isAdded = b.Add(name, date);
                                if(isAdded == false)
                                    Console.WriteLine("Запись с таким именем уже существует");
                            
                                break;

                            case (int)Comands.Remove:
                            
                                Console.Write("Введите имя: ");
                                string nameToRemove = Console.ReadLine();
                                
                                bool isRemoved = b.Remove(nameToRemove);
                                if(isRemoved == false)
                                    Console.WriteLine("Запись с таким именем не была найдена");
                        
                                break;

                            case (int)Comands.Nearest:
                            
                                b.PrintNear();
                        
                            break;

                            case (int)Comands.Change:
                                Console.Write("Введите имя: ");
                                string nameToChange = Console.ReadLine();
                                
                                Console.Write("Введите новое имя: ");
                                string newName = Console.ReadLine();
                                
                                Console.Write("Введите новую дату в формате [dd.mm]: ");
                                string newDate = Console.ReadLine();

                                bool IsChanged = false;
                                IsChanged = b.Change(nameToChange, newName, newDate);
                                if(IsChanged){
                                    Console.WriteLine("Данные были изменены");
                                    Console.WriteLine();
                                }else{
                                    Console.WriteLine("Данные не были изменены");
                                    Console.WriteLine();
                                }
                                break;
                        
                            case (int)Comands.Exit:
                            
                                b.WriteToFile(fileName);
                                Console.WriteLine("Данные были сохранены");
                            
                                break;
                            default :
                            
                                Console.WriteLine("Команда не распознана");
                            
                                break;  
                        }
                    }
   
                }        
		}

        private static bool IsArgumentCorrect(string input){
            
            int argument;
            try
            {
                argument = Convert.ToInt32(input);
            }

            catch(Exception e)
            {
                return false;
            }
            if(!(argument > 0 && argument <= 6 )){
                return false;
            }  
            return true; 
        }

        private bool IsNameCorrect(string name){
            if(name.Length == 0)
                return false;
            return true;
        }

        private bool IsDateCorrect(string date){
            if(date.Length != 5)
                return false;
            
            if(date.IndexOf(".") != 2)
                return false;
            
            string Digits = date.Replace(".", "");
            foreach(char elem in Digits){
                if(Char.IsDigit(elem) == false)
                    return false;
            }

            int day = int.Parse(date.Substring(0,2));
            int month = int.Parse(date.Substring(3));
            if(!(day > 0 && day <= 31 && month > 0 && month <= 12))
                return false;
            
            return true;
            
            
            
        }
    }

    class Birthdays
    {
		Dictionary<string, string> birthdays;

        public Birthdays()
        {
            birthdays = new Dictionary<string, string>();
        }

        public bool Add(string name, string date)
        {
            bool isAdded = birthdays.TryAdd(name, ReverseDate(date));
            return isAdded;
        }
        
        public int PrintAll()
        {    
            if(birthdays.Count == 0){
                Console.WriteLine("Список пока пуст!");
                Console.WriteLine();
                return 1;    
            }
            Console.WriteLine("Текущий список всех ДР: \n");
            foreach(string key in birthdays.Keys)
                Console.WriteLine(key + " - " + ReverseDate(birthdays[key]));
            return 0;
        }
        
        public bool Remove(string name)
        {
            bool isContains = birthdays.ContainsKey(name);
            if(isContains){
                birthdays.Remove(name);
                return true;
            }
            return false; 
        }

        public int PrintNear()
        {
            if(birthdays.Count == 0){
                Console.WriteLine("Список пока пуст!");
                Console.WriteLine();
                return 1;
            }

            Console.WriteLine("Список ближайших дней рождений:\n");
            
            int Dates = 0;
            string today = DateTime.Now.ToString("MM.dd");
            Dictionary<string, string>.KeyCollection keyColl = birthdays.Keys;

            foreach(string key in keyColl){
                if(birthdays[key] == today)
                    Console.WriteLine(key + " - Сегодня");
                    Dates++;
            }

            foreach(string key in keyColl){
                string edge = AddDays(today, 30);
                    //Console.WriteLine("value: {0} | edge: {1} | Comparing of value to edge result: {2} | Comparing of value to today result: {3}", birthdays[key], edge, birthdays[key].CompareTo(edge), birthdays[key].CompareTo(today));      
                        if(birthdays[key].CompareTo(edge) < 0 && birthdays[key].CompareTo(today) > 0){
                            Console.WriteLine(key + " - " + ReverseDate(birthdays[key]));
                            Dates++;
                        }
            }
            if(Dates == 0){
                Console.WriteLine("В ближайшее время день рождений нет!");
                return 1;
            }
            Console.WriteLine();
            return 0;                 
        }

        public bool Change(string name, string newName, string newDate){
            if(birthdays.ContainsKey(name)){
                birthdays.Remove(name);
                birthdays.Add(newName, ReverseDate(newDate));
                return true;
            }
            return false;
        }

        public bool ReadFromFile(string fileName){
            
            string txt = "";
            
            try{
                txt = File.ReadAllText("data.txt");
            }
            
            catch(Exception e){
                return false;
            }
            
            string[] words = txt.Split(new char[] { '\n' });

            foreach (string s in words)
            {
                int index = s.IndexOf(" ");

                string name = s.Substring(0, index);
                string date = s.Substring(index + 1);

                birthdays.Add(name, ReverseDate(date));
            }

            return true;
        }

        public void WriteToFile(string fileName){
            
            string buffer = "";
            foreach(string key in birthdays.Keys)
                    buffer += key + " " + ReverseDate(birthdays[key]) + "\n";
            File.WriteAllText(fileName, buffer);
        }

        private string ReverseDate(string s )
        {
            string leftPart = s.Substring(0,2);
            string rightPart = s.Substring(3);
            return rightPart + "." + leftPart;
        }
        
        private string AddDays(string s, int days)
        {
            int month = Convert.ToInt32(s.Substring(0,2));
            //Console.WriteLine("month: {0}", month);
            int day = Convert.ToInt32(s.Substring(3));
            //Console.WriteLine("day: {0}", day);
            int remainder = (day + days) - DateTime.DaysInMonth(DateTime.Now.Year, month);
            //Console.WriteLine("remainder: {0}", remainder);
            if(remainder == 0)
                day += days;
            else{
                day = remainder;
                month = (month + 1) % 12; 
            }
            //Console.WriteLine("result: " + month.ToString("D2") + "." + day.ToString("D2"));
            return month.ToString("D2") + "." + day.ToString("D2");
        }

        private void AddDaysTest(){
            string[] variants = {"01.01", "05.02", "20.01", "23.12"};
            string[] results = {"31.01", "07.03", "19.02", "22.01"};
            const int days = 30;
            for(int i = 0; i < variants.Count(); i++){
                string result =  ReverseDate(AddDays(ReverseDate(variants[i]),days));
                Console.WriteLine("Test #{0} | Input: \"{1}\" | Should be: \"{2}\" | Output: \"{3}\" - ", i+1, variants[i], results[i], result);
                if(result.CompareTo(results[i]) == 0)
                    Console.WriteLine("Ok");
                else
                    Console.WriteLine("WRONG");
            }
        }
    }
}
