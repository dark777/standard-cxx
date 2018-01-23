/*
 * http://www.cplusplus.com/reference/ostream/ostream/tellp/
 * http://www.dreamincode.net/forums/topic/39499-2-parts-of-attention-required/
 * https://cal-linux.com/tutorials/strings.html
 * https://scs.senecac.on.ca/~oop244/pages/content/files.html
 * https://www.vivaolinux.com.br/artigo/Manipulacao-de-arquivos-em-C++?
*/

#include <fstream>
#include <sstream>
#include "email.hxx"

const int MAX = 101;
Contact listContact[MAX];
int ContactCounter;

   void loadNumber(std::string& s, const std::string& label, uint16_t requiredSize)
   {
      do{
         if(label == "")
         {
          std::cout << "\n\tInvalid \"\" Label.\n";
          break;
         }
         else
         if(label == " ")
         {
          std::cout << "\n\tInvalid \" \" Label.\n";
          break;
         }
         else
         std::cout << "\n\tEnter " << label << "(" << requiredSize << " digits): ";
         getline(std::cin, s);
         
       }while(s.length() != requiredSize || label == "" || label == " ");
   }
     
   void loadString(std::string& s, const std::string& label)
   {
      do{
         if(label == "")
         {
          std::cout << "\n\tInvalid \"\" Label.\n";
          break;
         }
         else
         if(label == " ")
         {
          std::cout << "\n\tInvalid \" \" Label.\n";
          break;    
         }
         else 
         std::cout << "\n\tEnter " << label << ": ";
         getline(std::cin, s);
 
        }while(s.length() == 0 || label == "" || label == " ");
   }
   
   Contact getCheckDays(Contact& c)
   {
    int dia, mes, ano; 
    int months[12] = {31,28,31,30,31,30,31,31,30,31,30,31};
     
       do{
          loadNumber(c.dobYear, "Year of Birth", 4);
          ano = std::stoi(c.dobYear.c_str());
          
          if(ano < 1870)
           std::cout<<"\n\tYear "<<ano<<", can not be less than 1870.\n";
          else
          if(ano > 2100)    
           std::cout<<"\n\tYear "<<ano<<", can not be greater than 2100.\n";
            
          }while(ano < 1870 || ano > 2100);
       
       if(__isleap(ano))months[1] = 29;

       do{
          loadNumber(c.dobMonth, "Month of Birth", 2);
          mes = std::stoi(c.dobMonth.c_str());
          
          if(mes > months[mes-1])
           std::cout<<"\n\tThe month "<<mes<<" does not exist.\n";
         
         }while(mes > months[mes-1]);
       
       do{
          loadNumber(c.dobDay, "Day of Birth", 2);
          dia = std::stoi(c.dobDay.c_str());
            
          if(dia > months[mes-1])std::cout<<"\n\tThe month "<<mes<<" does not have "<<dia<<" days!!!\n\n";
         
         }while(dia > months[mes-1]);
       
      return c;
   }
   
   void loadStringMail(std::string& s, const std::string& label)
   {
      do{
         if(label == "")
         {
          std::cout << "\n\tInvalid \"\" Label.\n";
          break;
         }
         else
         if(label == " ")
         {
          std::cout << "\n\tInvalid \" \" Label.\n";
          break;    
         }
         else
          std::cout << "\n\tEnter " << label << ": ";
          getline(std::cin, s);
        
          std::size_t arroba(s.find("@")); //busca até encontrar arroba
          std::string dominio(s.substr(arroba+1)); //pega substring após o arroba
 
          std::size_t ponto(dominio.find("."));
          std::string subdominio(dominio.substr(ponto+1));

          if(s.find("@") > s.length() && (subdominio.find(".") < s.length())) //usuarioyahoo.com.br
          std::cout<<"\n\tEmail: "<<s<<(email(s).isMail() ? "\n": " is Invalid \'@\' not found ..!!!\n");
          else
          if(subdominio.find(".") > subdominio.length() && (s.find("@") < s.length())) //usuario@yahoocom.br && usuario@yahoo.combr
          std::cout<<"\n\tEmail: "<<s<<(email(s).isMail() ? "\n": " is Invalid \'.\' not found ..!!!\n");
          else
          if((s.find("@") > dominio.length()) && (subdominio.find(".") > subdominio.length())) //usuarioyahoocombr
          std::cout<<"\n\tEmail: "<<s<<(email(s).isMail() ? "\n": " is Invalid \'@\' e \'.\' not found ..!!!\n");
            
       }while(email(s).isMail() == 0 || label == "" || label == " ");  
   }
   
   Contact newContact() 
   {
    Contact c;
    
     //loadString(c.name, "Name");
     //loadString(c.address, "Address");
     //loadString(c.neighborhood, "Neighborhood");
     //loadString(c.HouseNumber, "House Number");
     //loadString(c.PhoneHomeNumber, "Phone Home Number");
     //loadString(c.PhoneWorkNumber, "Phone Work Number");
     //loadString(c.MobileNumber, "Mobile Number"); //Mobile Number
     loadStringMail(c.email,"Email Address");
     //getCheckDays(c);
       
     return c;
   }
   /* 
   Contact newContact()
   {
    Contact c;
    int dia, mes, ano;
    int months[12] = {31,28,31,30,31,30,31,31,30,31,30,31};
    
     do{
        std::cout << "\n\tEnter name complete: ";
        getline(std::cin, c.name);
          
        if(c.name.length() != 0) break;
            
        std::cout << "\n\tInvalid Name.\n";        
       }while(c.name.length() == 0);
         
     do{
        std::cout << "\n\tEnter email: ";
        getline(std::cin, c.email);
          
        std::cout<<email(c).print();  
       }while(email(c).isMail() == false);
       
     do{
        std::cout << "\n\tEnter house number: ";
        getline(std::cin, c.HouseNumber);
          
        if(c.HouseNumber.length() != 0)break;
            
        std::cout << "\n\tInvalid House Number.\n";      
       }while(c.HouseNumber.length() == 0);
         
    do{
       std::cout << "\n\tEnter neighborhood(bairro): ";
       getline(std::cin, c.neighborhood);
          
       if(c.neighborhood.length() != 0)break;
          
       std::cout << "\n\tInvalid Neighborhood(bairro).\n";       
      }while(c.neighborhood.length() == 0);
         
    do{
       std::cout << "\n\tEnter phone home number: ";
       getline(std::cin, c.PhoneHomeNumber);
          
       if(c.PhoneHomeNumber.length() == 14)break;
          
       std::cout << "\n\tInvalid Phone Home Number.\n";        
      }while(c.PhoneHomeNumber.length() == 0 || c.PhoneHomeNumber.length() < 14);
       
    do{
       std::cout << "\n\tEnter phone work number: ";
       getline(std::cin, c.PhoneWorkNumber);
          
       if(c.PhoneWorkNumber.length() == 14)break;
          
       std::cout << "\n\tInvalid Phone Work Number.\n";        
      }while(c.PhoneWorkNumber.length() == 0 || c.PhoneWorkNumber.length() < 14);
         
    do{
       std::cout << "\n\tEnter mobile number: ";
       getline(std::cin, c.MobileNumber);
          
       if(c.MobileNumber.length() == 15)break;
          
       std::cout << "\n\tInvalid Mobile Number.\n";
      }while(c.MobileNumber.length() == 0 || c.MobileNumber.length() < 15);
      
    do{
       
       do{
          std::cout << "\n\tEnter day of birth(2 digits): ";
          getline(std::cin, c.dobDay);
          
          if(c.dobDay.length() == 2)break;
          
          std::cout<< "\n\t" <<c.dobDay.length()<< " Invalid Birth Day.\n";
         }while (c.dobDay.length() == 0 || c.dobDay.length() != 2);
         
       do{
          std::cout << "\n\tEnter month of birth(2 digits): ";
          getline(std::cin, c.dobMonth);
          
          if(c.dobMonth.length() == 2)break;
          
          std::cout<< "\n\t" <<c.dobMonth.length()<< " Invalid Birth Month.\n";
         }while(c.dobMonth.length() == 0 || c.dobMonth.length() != 2);
       
       do{
          std::cout << "\n\tEnter year of birth(4 digits): ";
          getline(std::cin, c.dobYear);
          
          if(c.dobYear.length() == 4)break;
          
          std::cout<< "\n\t" <<c.dobYear.length()<< " Invalid Birth Year.\n";
         }while (c.dobYear.length() == 0 || c.dobYear.length() != 4);
       
         dia=std::stoi(c.dobDay);
         mes=std::stoi(c.dobMonth);
         ano=std::stoi(c.dobYear);
         
         if(__isleap(ano))months[1] = 29;
         
         if(dia > months[mes-1])std::cout<<"\n\tThe month "<<mes<<" of the year "<<ano<<" does not have "<<dia<<" days!!!\n\n";
         
      }while(dia > months[mes-1]); 
      
     return c;
   } 
  */
   void writeContacts(Contact& c, std::ofstream& os)
   {
    c.datetime = c.date.currentDateTime();
    
    os << "#\n"
       << "# Personal Name............: " << c.name << "\n"
       << "# House Address............: " << c.address << "\n"
       << "# Neighborhood.............: " << c.neighborhood << "\n"
       << "# House Number.............: " << c.HouseNumber << "\n"
       << "# Phone Home Number........: " << c.PhoneHomeNumber << "\n"
       << "# Phone Work Number........: " << c.PhoneWorkNumber << "\n"
       << "# Mobile Number............: " << c.MobileNumber << "\n"
       << "# Personal Email Adress....: " << c.email << "\n"
       << "# Extensive Birth..........: " << c.dobDay <<"/"<< c.dobMonth <<"/"<< c.dobYear << "\n"
       << "# DateTime Create Profile..: " << c.datetime;
   }
     
   void readContacts(Contact& c, std::ifstream& is)
   {
    getline(is, c.name);
      getline(is, c.address);
        getline(is, c.neighborhood);
         getline(is, c.HouseNumber);
          getline(is, c.PhoneHomeNumber);
           getline(is, c.PhoneWorkNumber);
           getline(is, c.MobileNumber);
          getline(is, c.email);
         getline(is, c.dobYear);
        getline(is, c.dobMonth);
      getline(is, c.dobDay);
    getline(is, c.datetime);
   }
     
   void addContact()
   {
    listContact[ContactCounter] = newContact();
    ContactCounter++;
   }
     
   void display(Contact& c)
   {
    std::cout << "\n\tContact Data.............: " << ContactCounter
              << "\n\tPersonal Name............: " << c.name
              << "\n\tHouse Address............: " << c.address
              << "\n\tNeighborhood.............: " << c.neighborhood
              << "\n\tHouse Number.............: " << c.HouseNumber
              << "\n\tPhone Home Number........: " << c.PhoneHomeNumber
              << "\n\tPhone Work Number........: " << c.PhoneWorkNumber
              << "\n\tMobile Number............: " << c.MobileNumber
              << "\n\tEmail Address............: " << c.email
              << "\n\tDate of Birth............: " << c.dobDay <<"/"<< c.dobMonth <<"/"<< c.dobYear
              << "\n\tDateTime Create Profile..: " << c.datetime<<"\n";
   }
   
   void display(Contact& c, std::ifstream& is)
   {
    std::cout << "\n\tContact Data: "
              << "\n\tName          - " << c.name
              << "\n\tAddress       - " << c.address
              << "\n\tNeighborhood  - " << c.neighborhood
              << "\n\tHome Number   - " << c.HouseNumber
              << "\n\tWork Number   - " << c.PhoneWorkNumber
              << "\n\tMobile Number - " << c.MobileNumber
              << "\n\tEmail Address - " << c.email
              << "\n\tDate of Birth - " << c.dobDay <<"/"<< c.dobMonth <<"/"<< c.dobYear << "\n";
   }
   /*
   void displayContact()
   {
    int c;
    
     do{
        std::cout << "\n\tEnter number of contact: ";
        std::cin >> c;
       }while(c<0 || c>=ContactCounter);
        
     display(listContact[c]);
     std::cout << "\n\tContact index ("<<ContactCounter<<") loaded and opened in the list.\n\n";
   }
   */
   void displayContact()
   {
    int ContactNumber;
    
    do{
       std::cout << "\n\tEnter number of the contact : ";
       std::cin >> ContactNumber;
      }while(ContactNumber < 0 || ContactNumber >= ContactCounter);
       
      if(ContactCounter > ContactNumber >= 0)
      {
       display(listContact[ContactNumber]);
       std::cout << "\n\tContact index ("<<ContactNumber<<") loaded and opened in the list.\n\n";
      }
     else
     std::cout << "Contact index ("<<ContactNumber<<") not found.\n\n";
   }
     
   void displayAll()
   {
    for(int i=0; i<ContactCounter; i++)
    display(listContact[i]);
   }
     
   void editContact()
   {
    int c;
    
    do{
       std::cout << "\n\tEnter number of contact: ";
       std::cin >> c;
      }while(c<0 || c>=ContactCounter);
        
     listContact[c] = newContact();
     std::cout << "\n\tEdit Contact("<<c<<") Details.\n\n";
   }
     
   void readFile()
   {
    try{
        std::ifstream inFile;
        inFile.open("ex9.txt", std::ios::in);
        inFile >> ContactCounter;
        inFile.ignore();
            
        for(int i=0; i<ContactCounter; i++)
        //display(listContact[i]);
        display(listContact[i], inFile);  
        inFile.close();
        inFile.clear();     
       }
       catch(std::exception& e) 
       {
        std::cerr << "\n\t<-|Error|-> Could not read file: "<<e.what()<<"\n";
       }
   }
     
   void writeFile()
   {
    try{
        std::ofstream outFile("ex10.txt",std::ofstream::ate|std::ofstream::app);
          //std::ofstream outFile("ex10.txt",std::ofstream::out|std::ofstream::app);
            
        if(outFile.is_open())
        {
         outFile <<"########################################################################";
         outFile <<"\n# Contact Data.............: "<<ContactCounter<<"\n";
         for(int i=0; i<ContactCounter; i++)
         writeContacts(listContact[i], outFile);
         //outFile <<"########################################################################\n\n";
        }
        outFile.close();
        outFile.clear();
        std::cout << "\n\tContact index ("<<ContactCounter<<") saved in list.\n\n";
       }
       catch(std::exception& e) 
       {
        std::cerr << "\n\t<-|Error|-> Could not write file: "<<e.what()<<"\n";
       }
   }
     
   void doMenu()
   {
    int op;
    Contact c;
    std::cout << "\n\tThe date is "  << c.date.toString()
              << "\n\n\tThe time is "<< c.time.toString()
              << "\n\n";
    do{
       std::cout << "\n\tEnter your selection."
                    "\n\t1. Add a Contact"
                    "\n\t2. Load Contacts List"
                    "\n\t3. Save Contacts List"
                    "\n\t4. Display Contact(s) Details"
                    "\n\t5. Display All Contact(s) Details"
                    "\n\t6. Edit Contact(s) Details"
                    "\n\t7. Quit"
                    "\n\tEscolha: ";
           
       std::cin >> op;
          
       std::cin.ignore();          
 
       switch(op)
       {
        case 1: 
        addContact();
        break;
        
        case 2: 
        readFile();
        break;
  
        case 3: 
        writeFile();
        break;

        case 4: 
        displayContact();
        break;

        case 5: 
        displayAll();
        break;

        case 6: 
        editContact();
        break;
   
        case 7: 
        std::cout<<"\n\tGood Bye ...!!!!\n\n";
        break;

        default:
        std::cout<<"\n\tOpção Inválida.\n\n";
       }
       
    }while(op != 7);
   }