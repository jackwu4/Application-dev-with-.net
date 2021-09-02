using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;


namespace NetAssignment1
{
    public abstract class Menu
    {

        //make all display text display in the centre of the line
        public void writeCentre(string text)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}", text));
        }


    }

    //Login Menu
    public class LoginMenu : Menu
    {
        private int posUsernameLeft, posUsernameTop, posPasswordLeft, posPasswordTop;                                   //console location attributes
        private string username, password;
        private string[] file;                                                                                          
        public LoginMenu()
        {
            do
            {
                setupMenu();
                enterDetail();
                readFile();
            } while (!matchLogin());
            Console.WriteLine("\n\n");
            writeCentre("Valid credentials!... Please enter");
            Console.ReadKey();
        }

        //read the login file and save it as file attribute
        private void readFile()
        {
			try
			{
				file = File.ReadAllLines(@"Login\\Login.txt");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			
        }

        //creates the border and the design of the banking login menu
        private void setupMenu()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║        WELCOME TO SIMPLE BANKING SYSTEM        ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                 LOGIN TO START                 ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║       User Name: ");
            posUsernameLeft = Console.CursorLeft;
            posUsernameTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║       Password: ");
            posPasswordLeft = Console.CursorLeft;
            posPasswordTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
        }


        //allows user to start entering the login details
        private void enterDetail()
        {
            Console.SetCursorPosition(posUsernameLeft, posUsernameTop);
            username = Console.ReadLine();
            Console.SetCursorPosition(posPasswordLeft, posPasswordTop);
            password = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
        }

        //match username and password given from user to the login file
        private bool matchLogin()
        {
            foreach (string element in file)
            {
                string[] words = element.Split('|');
                if (words[0] == username && words[1] == password)
                    return true;
            }
            Console.WriteLine("\n\n");
            writeCentre("Invalid logins, Please Try Again!");
            Console.ReadKey();
            return false;
        }


    }

    //MainMenu
    public class MainMenu : Menu
    {
        int posChoiceLeft, posChoiceTop, userChoice;
        public MainMenu()
        {
            setupMenu();

            Console.ReadKey();
        }

        //Setup the displaying menu for the main menu
        private void setupMenu()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║        WELCOME TO SIMPLE BANKING SYSTEM        ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║         1. Create a new account                ║");
            writeCentre("║         2. Search for an account               ║");
            writeCentre("║         3. Deposit                             ║");
            writeCentre("║         4. Withdraw                            ║");
            writeCentre("║         5. A/C statement                       ║");
            writeCentre("║         6. Delete account                      ║");
            writeCentre("║         7. Exit                                ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            Console.Write("\t\t\t ║\t Enter your choice (1-7): ");
            posChoiceLeft = Console.CursorLeft;
            posChoiceTop = Console.CursorTop;
            Console.Write("\t\t  ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");

            enterChoice();
            functions();
        }

        //run the selected function
        private void functions()
        {
            switch (userChoice)
            {
                case 1: newAccount(); break;
                case 2: searchAccount(); break;
                case 3: deposit(); break;
                case 4: withdraw(); break;
                case 5: statement(); break;
                case 6: deleteAccount(); break;
                case 7: break;
                default: Console.WriteLine("Error"); break;

            }
        }

        //create a new account function
        private void newAccount()
        {
            newAccount n1 = new newAccount();
            n1.start();
            writeCentre("Press any key to continue...");
            Console.ReadKey();
            setupMenu();
        }

        //search a existing account
        private void searchAccount()
        {
            searchAccount s1 = new searchAccount();
            s1.start();
            setupMenu();
        }

        //make a deposit
        private void deposit()
        {
            Transaction t1 = new Transaction();
            t1.start("deposit");
            setupMenu();
        }

        //make a withdraw
        private void withdraw()
        {
            Transaction t1 = new Transaction();
            t1.start("withdraw");
            setupMenu();
        }

        //display the statement of the account
        private void statement()
        {
            Statement s1 = new Statement();
            s1.start();
            setupMenu();
        }

        //remove an existing account
        private void deleteAccount()
        {
            Delete d1 = new Delete();
            d1.start();
            setupMenu();
        }

        //allows user to enter a number to select the function they want to use
        private void enterChoice()
        {
            Console.SetCursorPosition(posChoiceLeft, posChoiceTop);
            string choice = Console.ReadLine();
            int temp = convertInput(choice);
            if (temp != -1)
                userChoice = temp;
            else
            {
                Console.WriteLine("\n\n");
                writeCentre("Please enter a valid number!");
                Console.ReadKey();
                setupMenu();
            }
        }

        //converting string input to int
        private int convertInput(string input)
        {
            int userInput;
            try
            {
                userInput = Convert.ToInt32(input);
            }
            catch (System.FormatException)
            {
                return -1;
            }

            if (userInput >= 1 && userInput <= 7)
                return userInput;
            else
                return -1;
        }
    }

    //new account function class
    public class newAccount : Menu
    {
        int firstNamePosTop, firstNamePosLeft, lastNamePosTop, lastNamePosLeft, addressPosTop,
            addressPosLeft, phonePosTop, phonePosLeft, emailPosTop, emailPosLeft;
        int messagePosTop, messagePosLeft;
        string firstName, lastName, address, email, phone;
        
        public newAccount()
        {

        }

        //runs the function in the correct order
        public void start()
        {
            while (true)
            {
                setupMenu();
                enterDetails();

                if (isInformationCorrect())
                {
                    Account a1 = new Account(firstName, lastName, address, phone, email);
                    a1.writeFile();
                    Console.WriteLine();
                    emailDetails();
                    Console.WriteLine();
                    showAccountNum(a1);
                    setCursorPos();
                    showMessage("Please Wait... Processing email..");
                    try
                    {
                        a1.sendEmail();
                    }
                    catch (System.Net.Mail.SmtpException)
                    {
                        clearMessage();
                        showMessage("No internet connection! Details cannot be sent");
                        Console.ReadKey();
                    }

                    clearMessage();
                    showMessage("Email Sent!");
                    break;
                }
            }
        }

        //creates the design interface for creating a new account
        private void setupMenu()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║              CREATE A NEW ACCOUNT              ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                ENTER THE DETAILS               ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║      First Name: ");
            firstNamePosLeft = Console.CursorLeft;
            firstNamePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║      Last Name: ");
            lastNamePosLeft = Console.CursorLeft;
            lastNamePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║      Address: ");
            addressPosLeft = Console.CursorLeft;
            addressPosTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║      Phone: ");
            phonePosLeft = Console.CursorLeft;
            phonePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║      Email: ");
            emailPosLeft = Console.CursorLeft;
            emailPosTop = Console.CursorTop;
            Console.Write("\t\t\t\t\t  ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;


        }

        //allows user to enter the details
        private void enterDetails()
        {
            Console.SetCursorPosition(firstNamePosLeft, firstNamePosTop);
            firstName = Console.ReadLine();
            Console.SetCursorPosition(lastNamePosLeft, lastNamePosTop);
            lastName = Console.ReadLine();
            Console.SetCursorPosition(addressPosLeft, addressPosTop);
            address = Console.ReadLine();
            bool printed = false;
            while (true)
            {
                Console.SetCursorPosition(phonePosLeft, phonePosTop);
                phone = Console.ReadLine();
                if (!isValidPhone(phone))
                {
                    showMessage("Please enter interger no longer than 10 digit");
                    //Console.WriteLine("\n\n\n");
                    //writeCentre("Please enter interger no longer than 10 digit");
                    printed = true;
                    Console.SetCursorPosition(phonePosLeft, phonePosTop);
                    Console.Write("\t\t\t\t\t  ║\t\t\t");
                }
                else
                    break;
            }
            if (printed)
                clearMessage();
                //Console.WriteLine("\n\n\n\n\t\t\t\t\t\t\t\t\t\t\t\t");
                
            printed = false;
            while (true)
            {
                Console.SetCursorPosition(emailPosLeft, emailPosTop);
                string tempEmail = Console.ReadLine();
                if (isValidEmail(tempEmail))
                {
                    email = tempEmail;
                    break;
                }
                else
                {
                    showMessage("Please enter a valid email address");
                    //Console.WriteLine("\n\n");
                    //writeCentre("Please enter a valid email address");
                    printed = true;
                    Console.SetCursorPosition(emailPosLeft, emailPosTop);
                    Console.WriteLine("\t\t\t\t\t  ║\t\t\t");
                }
            }
            if (printed)
                clearMessage();
        }

        //displays email detail message
        private void emailDetails()
        {
            writeCentre("Account Created! details will be provided via email.");
        }

        //display the account number of the created account
        private void showAccountNum(Account a1)
        {
            writeCentre("Account number is: " + a1.getAccountNum());


        }

        //check if the enter phone number is a valid input
        private bool isValidPhone(string input)
        {
            try
            {
                Convert.ToInt32(input);
            }
            catch (System.FormatException)
            {
                return false;
            }
            catch (System.OverflowException)
            {
                return false;
            }
            if (input.Length > 10)
                return false;
            else
                return true;
        }

        //check if the entered email is a valid email
        private bool isValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException )
            {
                return false;
            }
            catch (ArgumentException )
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        
        //allows user to check if the entered details are correct
        private bool isInformationCorrect()
        {
            showMessage("Is the information correct (Y/N) ");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //set the message displaying location
        private void setCursorPos()
        {
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
        }

        //show the message at a specific location
        private void showMessage(string message)
        {
            Console.SetCursorPosition(messagePosLeft, messagePosTop);
            writeCentre(message);
        }

        //clear the message which was shown
        private void clearMessage()
        {
            Console.SetCursorPosition(messagePosLeft, messagePosTop);
            writeCentre("\t\t\t\t\t\t\t\t");
        }

    }

    //search account function class
    public class searchAccount:Menu
    {
        protected int accountNumPosLeft, accountNumPosTop;
        protected int accountNumberPosLeft, accountNumberPosTop, firstNamePosTop, firstNamePosLeft, lastNamePosTop, lastNamePosLeft, addressPosTop,
            addressPosLeft, phonePosTop, phonePosLeft, emailPosTop, emailPosLeft, balancePosLeft, balancePosTop;
        protected int accountNumber;
        
        protected int messagePosLeft, messagePosTop;
        protected Account a1;
        public searchAccount()
        {
            
        }

        //starting point
        public void start()
        {
            do
            {
                setupMenu();
                enterDetail();
            } while (checkAnother());
        }

        //setup the interface
        private void setupMenu()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║                SEARCH AN ACCOUNT               ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                ENTER THE DETAILS               ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║    Account Number: ");
            accountNumPosLeft = Console.CursorLeft;
            accountNumPosTop = Console.CursorTop;
            Console.Write("                            ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            Console.WriteLine();
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;

        }

        //allows user to enter details
        protected bool enterDetail()
        {
            Console.SetCursorPosition(accountNumPosLeft, accountNumPosTop);
            string userInput = Console.ReadLine();
            if (isValidAccountNumber(userInput))
            {
                accountNumber = Convert.ToInt32(userInput);
                if (isAccountExist(accountNumber))
                {
                    readAccountDetail();
                    showAccountDetail();
                    return true;
                }
                else
                {
                    //Console.SetCursorPosition(accountNumPosLeft, accountNumPosTop);
                    //Console.Write("\t\t\t\t  ║\t\t\t");
                    showMessage("Account not found! Press any key to continue.");
                    Console.ReadKey();                                              
                }
                    
            }
            else
            {
                showMessage("Invaild Account Format! Press any key to continue.");
                //Console.SetCursorPosition(accountNumPosLeft, accountNumPosTop);
                //Console.Write("\t\t\t\t  ║\t\t\t");
                Console.ReadKey();      
            }
            return false;
        }

        //asks user if wants to check another account
        protected bool checkAnother()
        {
            Console.Write("\t\t\t\tCheck another account (Y/N)? ");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                return true;
            }
            else
            {
                return false;
            }

        }

        //read account details for the search and save it in to 'a1' attribute
        protected void readAccountDetail()
        {
            string[] lines = File.ReadAllLines("Account//" + this.accountNumber + ".txt");
            //int accountNumber = Convert.ToInt32(lines[0]);
            //double balance = Convert.ToDouble(lines[1]);
            //string firstName = lines[2];
            //string lastName = lines[3];
            //string address = lines[4];
            //string phone = lines[5];
            //string email = lines[6];
            //a1 = new Account(accountNumber,balance,firstName,lastName,address,phone,email);
            a1 = new Account(lines);
        }

        //display account details in a interface like look
        protected void showAccountDetail()
        {
            Console.WriteLine();
            writeCentre("Account Found!");
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║                 ACCOUNT DETAILS                ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║    Account Number: ");
            accountNumberPosLeft = Console.CursorLeft;
            accountNumberPosTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║    Balance: ");
            balancePosLeft = Console.CursorLeft;
            balancePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║    First Name: ");
            firstNamePosLeft = Console.CursorLeft;
            firstNamePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║    Last Name: ");
            lastNamePosLeft = Console.CursorLeft;
            lastNamePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║    Address: ");
            addressPosLeft = Console.CursorLeft;
            addressPosTop = Console.CursorTop;
            Console.Write("\t\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║    Phone: ");
            phonePosLeft = Console.CursorLeft;
            phonePosTop = Console.CursorTop;
            Console.Write("\t\t\t\t\t  ║\n");
            Console.Write("\t\t\t ║    Email: ");
            emailPosLeft = Console.CursorLeft;
            emailPosTop = Console.CursorTop;
            Console.Write("\t\t\t\t\t  ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
            Console.SetCursorPosition(accountNumberPosLeft, accountNumberPosTop);
            Console.Write(a1.getAccountNum());
            Console.SetCursorPosition(balancePosLeft, balancePosTop);
            Console.Write(a1.getBalance());
            Console.SetCursorPosition(firstNamePosLeft, firstNamePosTop);
            Console.Write(a1.getFirst());
            Console.SetCursorPosition(lastNamePosLeft, lastNamePosTop);
            Console.Write(a1.getLast());
            Console.SetCursorPosition(addressPosLeft, addressPosTop);
            Console.Write(a1.getAddress());
            Console.SetCursorPosition(phonePosLeft, phonePosTop);
            Console.Write(a1.getPhone());
            Console.SetCursorPosition(emailPosLeft, emailPosTop);
            Console.Write(a1.getEmail());
            Console.WriteLine("\n");
            
        }

        //check if account number entered is a valid number
        protected bool isValidAccountNumber(string accountNumber)
        {
            try
            {
                Convert.ToInt32(accountNumber);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //check if account number entered is a valid account
        protected bool isAccountExist(int accountNumber)
        {
            string[] accounts = Directory.GetFiles("Account", "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();
            
            return Array.Exists(accounts, element => element == accountNumber + ".txt"); ;
        }

        //display the message
        protected void showMessage(string message)
        {
            Console.SetCursorPosition(messagePosLeft, messagePosTop);
            writeCentre(message);
        }

        //clear the message
        protected void clearMessage()
        {
            Console.SetCursorPosition(messagePosLeft, messagePosTop);
            writeCentre("\t\t\t\t\t\t\t");
        }
    }

    //this class supports both withdraw and deposit
    public class Transaction : Menu
    {
        private int accountNumPosLeft, accountNumPosTop, amountPosLeft, amountPosTop, messagePosLeft, messagePosTop;
        private int accountNumber;
        private double amount;
        private Account a1;
        public Transaction()
        {
            
        }

        //starting point, depending on action this class will act differently
        public void start(string action)
        {
            do
            {
                if (action == "deposit")
                    setupDeposit();
                if (action == "withdraw")
                    setupWithdraw();

                enterDetail(action);
            } while (checkAnother());
        }

        //setup the withdraw interface
        private void setupWithdraw()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║                    WITHDRAW                    ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                ENTER THE DETAILS               ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║    Account Number: ");
            accountNumPosLeft = Console.CursorLeft;
            accountNumPosTop = Console.CursorTop;
            Console.Write("                            ║\n");
            Console.Write("\t\t\t ║    Amount: $");
            amountPosLeft = Console.CursorLeft;
            amountPosTop = Console.CursorTop;
            Console.Write("                                   ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            Console.WriteLine();
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
        }

        //setup the deposit interface 
        private void setupDeposit()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║                     DEPOSIT                    ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                ENTER THE DETAILS               ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║    Account Number: ");
            accountNumPosLeft = Console.CursorLeft;
            accountNumPosTop = Console.CursorTop;
            Console.Write("                            ║\n");
            Console.Write("\t\t\t ║    Amount: $");
            amountPosLeft = Console.CursorLeft;
            amountPosTop = Console.CursorTop;
            Console.Write("                                   ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            Console.WriteLine();
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
        }

        //allows user to enter the details 
        private void enterDetail(string action)
        {         
            Console.SetCursorPosition(accountNumPosLeft, accountNumPosTop);
            string userInput = Console.ReadLine();
            if (isValidAccountNumber(userInput))
            {
                accountNumber = Convert.ToInt32(userInput);
                if (isAccountExist(accountNumber))
                {
                    showMessage("Account found! Enter the amount...");
                    readAccountDetail();
                    do
                    {
                        Console.SetCursorPosition(amountPosLeft, amountPosTop);
                        userInput = Console.ReadLine();
                        clearMessage();
                        if (isValidAmount(userInput))
                        {
                            amount = Convert.ToDouble(userInput);
                            if(action == "deposit")
                            {
                                a1.deposit(amount);
                                a1.writeFile();
                                showMessage("Deposit successful!");
                                break;
                            }
                                
                            if (action == "withdraw")
                            {
                                if (a1.withdraw(amount))
                                {
                                    a1.writeFile();
                                    showMessage("Withdraw successful!");
                                    break;
                                }
                                else
                                {
                                    showMessage("Not enough balance! Press any key to continue..");
                                    Console.ReadKey();
                                }
                            }
                                

                            
                        }
                        else
                        {
                            showMessage("Invalid Amount Format! Press any key to continue..");
                            Console.ReadKey();
                        }
                    } while (tryAmountAgain());
                    
                }
                else
                {
                    //Console.SetCursorPosition(accountNumPosLeft, accountNumPosTop);
                    //Console.Write("\t\t\t\t  ║\t\t\t");
                    showMessage("Account not found! Press any key to continue..");
                    Console.ReadKey();
                }
            }
            else
            {
                showMessage("Invalid Account Format! Press any key to continue..");
                //Console.SetCursorPosition(accountNumPosLeft, accountNumPosTop);
                //Console.Write("\t\t\t\t  ║\t\t\t");
                Console.ReadKey();
            }
        }

        //if amount entered is invalid, ask user if they wants to enter again
        private bool tryAmountAgain()
        {
            Console.Write("\t\t\t\t\tEnter amount again (Y/N)? ");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                Console.SetCursorPosition(amountPosLeft, amountPosTop);
                Console.WriteLine("                                   ║\t\t\t");
                return true;
            }
            else
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                return false;
            }
        }

        //asks user if user wants to do another transaction
        private bool checkAnother()
        {
            Console.Write("\t\t\t\tStart another transaction (Y/N)? ");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                return true;
            }
            else
            {
                return false;
            }

        }

        //read account details and save in to 'a1'
        private void readAccountDetail()
        {
            string[] lines = File.ReadAllLines("Account//" + this.accountNumber + ".txt");
            //int accountNumber = Convert.ToInt32(lines[0]);
            //double balance = Convert.ToDouble(lines[1]);
            //string firstName = lines[2];
            //string lastName = lines[3];
            //string address = lines[4];
            //string phone = lines[5];
            //string email = lines[6];
            a1 = new Account(lines);
        }

        //check if amount is valid
        private bool isValidAmount(string amount)
        {
            try
            {
                double temp = Convert.ToDouble(amount);
                if (temp < 0)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //check if account number is valid
        private bool isValidAccountNumber(string accountNumber)
        {
            try
            {
                Convert.ToInt32(accountNumber);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //check if account exists in folder
        private bool isAccountExist(int accountNumber)
        {
            string[] accounts = Directory.GetFiles("Account", "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();

            return Array.Exists(accounts, element => element == accountNumber + ".txt"); ;
        }

        private void showMessage(string message)
        {
            Console.SetCursorPosition(messagePosLeft, messagePosTop);
            writeCentre(message);
        }
        private void clearMessage()
        {
            Console.SetCursorPosition(messagePosLeft, messagePosTop);
            writeCentre("\t\t\t\t\t\t\t");
        }


    }

    //statement function class
    public class Statement : searchAccount
    {
        public Statement()
        {

        }

        //starting point
        public new void start()
        {
            do
            {
                setupMenu();
                if (enterDetail())
                {
                    showStatement();
                    if (emailStatment())
                    {
                        bool exception = false;
                        showMessage("Please Wait... Processing email..");
                        try
                        {
                            a1.sendEmail();
                        }
                        catch (System.Net.Mail.SmtpException)
                        {
                            exception = true;
                            clearMessage();
                            showMessage("No internet connection! Details cannot be sent");
                            Console.ReadKey();
                        }
                        if (!exception)
                        {
                            clearMessage();
                            showMessage("Email sent successfully");
                        }
                    }
                    
                        
                }
                    
                    
            } while (emailAnother());
        }

        //setup the interface
        private void setupMenu()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║                    STATEMENT                   ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                ENTER THE DETAILS               ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║    Account Number: ");
            accountNumPosLeft = Console.CursorLeft;
            accountNumPosTop = Console.CursorTop;
            Console.Write("                            ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            Console.WriteLine();
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
        }

        //display the statment
        private void showStatement()
        {
            String[] statements = a1.getStatements();
            
            foreach (string statement in statements)
            {
                Console.WriteLine("\t\t" + statement);
            }
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
        }

        //email the statement to user
        private bool emailStatment()
        {
            Console.Write("\t\t\t\tEmail statement (y/n)?");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                return true;
            }
            else
            {
                return false;
            }
        }

        //ask user if they want to check another account
        private bool emailAnother()
        {
            Console.Write("\n\t\t\t\tEmail another account statement (Y/N)? ");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //delete account function class
    public class Delete : searchAccount
    {
        public Delete()
        {
            
        }

        //starting point
        public new void start()
        {
            do
            {
                setupMenu();
                if (enterDetail())
                    deleteFile();
            } while (deleteAnother());
        }
        
        //setup the interface
        private void setupMenu()
        {
            Console.Clear();
            writeCentre("╔════════════════════════════════════════════════╗");
            writeCentre("║                DELETE AN ACCOUNT               ║");
            writeCentre("╠════════════════════════════════════════════════╣");
            writeCentre("║                ENTER THE DETAILS               ║");
            writeCentre("║                                                ║");
            Console.Write("\t\t\t ║    Account Number: ");
            accountNumPosLeft = Console.CursorLeft;
            accountNumPosTop = Console.CursorTop;
            Console.Write("                            ║\n");
            writeCentre("╚════════════════════════════════════════════════╝");
            Console.WriteLine();
            messagePosLeft = Console.CursorLeft;
            messagePosTop = Console.CursorTop;
        }

        //confirm if user wants to delete the account, then delete if user enter Y
        private void deleteFile()
        {
            showMessage("Delete (y/n)?");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                File.Delete("Account//"+accountNumber+".txt");
                
                Console.WriteLine("\t\t\t\tAccount Deleted!...");
            }
            
        }

        //ask user if there is another account wishs to be deleted
        private bool deleteAnother()
        {
            Console.Write("\t\t\t\tDelete another account (Y/N)? ");
            char userInput = Console.ReadKey().KeyChar;
            if (char.ToUpperInvariant(userInput) == 'Y')
            {
                Console.SetCursorPosition(messagePosLeft, messagePosTop);
                Console.WriteLine();
                writeCentre("\t\t\t\t\t\t\t");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //Account class, contain all attribute of the account, read and write functions, deposit,withdraw etc...
    public class Account
    {
        int accountNum;
        double balance = 0;
        string firstName, lastName, address, phone, email;
        string[] lines = new string[12];

        public Account()
        {
            
        }

        //contruct an account with full provided details and add in to txt format
        public Account(string first, string last, string address, string phone, string email)
        {
            lines[0] = generateAccountNum().ToString();
            lines[1] = balance.ToString();
            lines[2] = first;
            lines[3] = last;
            lines[4] = address;
            lines[5] = phone;
            lines[6] = email;
            lines[7] = String.Format("{0,-20} | {1,-10} |  {2,-10} |  {3,-20}", "Date/Time", "Type", "Amount", "Updated Balance");
        }

        //construct an account with arrays of lines which is extracted from txt file
        public Account(string[] lines)
        {
            this.lines = lines;
            this.balance = Convert.ToDouble(lines[1]);

        }

        //construct an account with full detail but save in account format
        public Account(int accountNum, double balance, string first, string last, string address, string phone, string email)
        {
            this.accountNum = accountNum;
            this.balance = balance;
            firstName = first;
            lastName = last;
            this.address = address;
            this.phone = phone;
            this.email = email;

        }

        //construct an account with only account number
        public Account(int accountNum)
        {
            this.accountNum = accountNum;
        }

        //deposit in to account and update lines
        public void deposit(double amount)
        {
            balance += amount;
            lines[1] = string.Format("{0:0.00}", balance);
            transaction("Deposit ", string.Format("{0:0.00}", amount));
        }

        //add transaction details
        public void transaction(string action, string amount)
        {
            int currentLength = lines.Length;
            List<string> list = lines.ToList();
            if (currentLength == 13)
            {
                list.RemoveAt(8);
            }
            list.Add(String.Format("{0,-20} | {1,-10} | ${2,10} | ${3,20}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), action, amount, getBalance()));
            //list.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\t" + action + "\t$" + amount + "\t\t$" + getBalance());
            lines = list.ToArray();
        }

        //withdraw out of accoount and update lines
        public bool withdraw(double amount)
        {
            if (amount > balance)
            {
                return false;
            }
            else
            {
                balance -= amount;
                lines[1] = string.Format("{0:0.00}", balance);
                transaction("Withdraw", string.Format("{0:0.00}", amount));
                return true;
            }
        }

        //write lines in to file
        public void writeFile()
        {
            //File.WriteAllText("Account//" + accountNum + ".txt", accountNum.ToString());
            //File.AppendAllText("Account//" + accountNum + ".txt", "\n" + balance.ToString());
            //File.AppendAllText("Account//" + accountNum + ".txt", "\n" + firstName);
            //File.AppendAllText("Account//" + accountNum + ".txt", "\n" + lastName);
            //File.AppendAllText("Account//" + accountNum + ".txt", "\n" + address);
            //File.AppendAllText("Account//" + accountNum + ".txt", "\n" + phone.ToString());
            //File.AppendAllText("Account//" + accountNum + ".txt", "\n" + email);
            File.WriteAllLines("Account//" + getAccountNum() + ".txt", lines);

        }

        //generate an account number for new accounts
        private int generateAccountNum()
        {
            //string[] pdfFiles = Directory.GetFiles("Account", "*")
            //                         .Select(Path.GetFileName)
            //                         .ToArray();
            //string[] fileName = pdfFiles[pdfFiles.Length - 1].Split('.');
            //return Int32.Parse(fileName[0]) + 1;
            int accountNum = Convert.ToInt32(File.ReadAllText("Account//AccountNum.txt")) + 1;
            File.WriteAllText("Account//AccountNum.txt", accountNum.ToString());
            return accountNum;

        }

        public string getAccountNum()
        {
            return lines[0];
        }

        public string getBalance()
        {
            return lines[1];
        }

        public string getFirst()
        {
            return lines[2];
        }

        public string getLast()
        {
            return lines[3];
        }

        public string getAddress()
        {
            return lines[4];
        }

        public string getPhone()
        {
            return lines[5];
        }

        public string getEmail()
        {
            return lines[6];
        }

        public string[] getStatements()
        {
            List<string> list = lines.ToList();
            list.RemoveRange(0, 7);
            return list.ToArray();
        }

        public void sendEmail()
        {
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            { 
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("dummytesting335@gmail.com", "jackwucz1212");
                SmtpServer.EnableSsl = true;
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("dummytesting335@gmail.com");
                    mail.To.Add(getEmail());
                    mail.Subject = "Account Detail/Statement";
                    mail.Body = "Detail is in attachment";

                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment("Account//" + getAccountNum() + ".txt");
                    mail.Attachments.Add(attachment);
                    SmtpServer.Send(mail);
                }
            }

        }
    }
}
        
