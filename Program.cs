using System.Globalization;
using System.Timers;

List<Plant> plants = new List<Plant>()
{
    new Plant()
    {
        Species = "Spider Plant",
        LightNeeds = 3,
        AskingPrice = 10.00M,
        City = "Nashville",
        ZIP = 37115,
        Sold = false,
        AvailableUntil = new DateTime(2024, 1, 11)
    },
    new Plant()
    {
        Species = "Aloe Vera",
        LightNeeds = 4,
        AskingPrice = 5.00M,
        City = "Boston",
        ZIP = 11045,
        Sold = false,
        AvailableUntil = new DateTime(2025, 1, 12)

    },
    new Plant()
    {
        Species = "Snake Plant",
        LightNeeds = 4,
        AskingPrice = 10.00M,
        City = "Asheville",
        ZIP = 74631,
        Sold = false,
        AvailableUntil = new DateTime(2025, 1, 12)

    },
    new Plant()
    {
        Species = "Christmas Cactus",
        LightNeeds = 2,
        AskingPrice = 8.00M,
        City = "Pasadena",
        ZIP = 92331,
        Sold = false,
        AvailableUntil = new DateTime(2025, 1, 12)
    },
    new Plant()
    {
        Species = "Baby Toes",
        LightNeeds = 2,
        AskingPrice = 5.00M,
        City = "Brooklyn",
        ZIP = 11002,
        Sold = true,
        AvailableUntil = new DateTime(2025, 1, 12)
    }
};

string greeting = "Welcome to Extra-Vert: your second-hand plant shop";

Console.WriteLine(greeting);

MainMenu();

void DisplayPlants()
{
    for (int i = 0; i < plants.Count; i++)
    {
        Console.WriteLine(PlantDetails(plants[i]));
    }
}

void AddAPlant()
{
    Console.WriteLine("To add a plant, please enter plant species");

    string responseSpecies = Console.ReadLine();

    Console.WriteLine("Please enter the asking price");

    decimal responsePrice = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Please enter city where plant is located");

    string responseCity = Console.ReadLine();

    Console.WriteLine("Please enter ZIP code");

    int responseZip = int.Parse(Console.ReadLine());

    Console.WriteLine(@"Please rate your plant's light needs on a scale from 1 to 5:
    1. Full shade
    2. Part shade
    3. Neutral
    4. Part sun
    5. Full sun");

    int responseLight = int.Parse(Console.ReadLine());

    Console.WriteLine("Please enter a date (MM/DD/YYYY) your post will expire");

    string response = Console.ReadLine();

    DateTime dateValue;
    if (!DateTime.TryParse(response, out dateValue))
    {
        Console.WriteLine("Not a valid format");
        MainMenu();
    };
   
   
    Plant newPlant = new Plant
    {
        Species = responseSpecies,
        AskingPrice = responsePrice,
        Sold = false,
        LightNeeds = responseLight,
        City = responseCity,
        ZIP = responseZip,
        AvailableUntil = dateValue

    };

    plants.Add(newPlant);

    Console.WriteLine(@$"You added:");
    Console.WriteLine(PlantDetails(newPlant));
}

void AdoptAPlant()
{
    Console.WriteLine("Plants available for adoption:");

    List<Plant> unsoldPlants = new List<Plant>();

    unsoldPlants = plants.Where(p => p.Sold == false && p.AvailableUntil > DateTime.Now).ToList();

    if (unsoldPlants.Count == 0 )
    {
        Console.WriteLine("No plants are currently available");
    } else
    {
        for (int i = 0; i < unsoldPlants.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {unsoldPlants[i].Species}");
        }

        Console.WriteLine("Please enter number beside the plant you wish to adopt");

        int choice = int.Parse(Console.ReadLine().Trim());

        unsoldPlants[choice - 1].Sold = true;

        Console.WriteLine($"You adopted {unsoldPlants[choice - 1].Species}. Congratulations!");
    }
}

void DelistAPlant()
{
    Console.WriteLine("Please select a plant to remove");

    for (int i = 0; i < plants.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {plants[i].Species}");
    }

    Plant chosenPlant = null;
    string response = "";

    while (chosenPlant == null)
    {
        Console.WriteLine("Please enter a plant number: ");
        try
        {
            int answer = int.Parse(Console.ReadLine().Trim());

            chosenPlant = plants[answer - 1];

            Console.WriteLine(@$"Are you sure you want to delist {chosenPlant.Species}?
                1. Yes, delist it.
                2. No, cancel.");

            try
            {
                response = Console.ReadLine();
                if (response == "1")
                {
                    plants.RemoveAt(answer - 1);
                    Console.WriteLine($"You have removed {chosenPlant.Species}");
                    MainMenu();
                }
                else if (response == "2")
                {
                    MainMenu();
                }
            }
            catch
            {
                Console.WriteLine("Please select 1 for delist and 2 for cancel");
            }

        }
        catch (FormatException)
        {
            Console.WriteLine("Please type only integers!");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Please choose an existing plant only!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("Try again!");
        }
    }

}

void PlantOfTheDay()
{
    List<Plant> unsoldPlants = plants.Where(x => x.Sold == false).ToList();

    Random random = new Random();
    int randomInt = random.Next(0, unsoldPlants.Count);
    Plant randomPlant = unsoldPlants[randomInt];

    Console.WriteLine($@"The plant of the day is:");
    Console.WriteLine(PlantDetails(randomPlant));

}

void SearchPlants()
{
    Console.WriteLine(@"Please enter a number from 1-5 to indicate the maximum light needs rating you are looking for:
    (1 being full shade, 5 being full sun)");

    int lightNeed = int.Parse(Console.ReadLine());

    List<Plant> plantsByLight = plants.Where(p => p.LightNeeds <= lightNeed).ToList();

    if (plantsByLight.Count == 0)
    {
        Console.WriteLine("No plants match your search");
    } else
    {
        foreach (Plant plant in plantsByLight)
        {
            Console.WriteLine($"{plant.Species}");
        }
    }
    plantsByLight.Clear();

}

void ViewStats()
{
    Plant cheapestPlant = plants.OrderByDescending(p => p.AskingPrice).FirstOrDefault();
    string cheapestPlantName = cheapestPlant.Species;

    int numAvailPlants = plants.Where(p => p.Sold == false && p.AvailableUntil > DateTime.Now).Count();

    Plant mostLight = plants.OrderByDescending(p => p.LightNeeds).FirstOrDefault();
    string mostLightName = mostLight.Species;

    List<int> lightNeeds = plants.Select(p => p.LightNeeds).ToList();
    double lightAverage = lightNeeds.Average();

    int adoptedPlants = plants.Where(p => p.Sold).Count();
    double adoptedPlantsAsDouble = (double)adoptedPlants;
    int allPlants = plants.Count;
    double percentAdopted = adoptedPlantsAsDouble/allPlants;
    string percentage = String.Format("Value: {0:P2}.", percentAdopted);
    
    Console.WriteLine($@"Stats:
Lowest price plant Species: {cheapestPlantName}
Number of plants available: {numAvailPlants}
Species with highest light needs: {mostLightName}
Average light needs: {lightAverage}
Percentage of plants adopted: {percentage}");
}

string PlantDetails(Plant plant)
{
    string plantString = @$"A {plant.Species} in {plant.City}, with a light needs rating of {plant.LightNeeds},
    {(plant.Sold ? "was sold" : "is available")} for {plant.AskingPrice} dollars.
    This post is {(plant.AvailableUntil > DateTime.Now ? "still valid" : "expired")}.";

    return plantString;
}
void MainMenu()
{
    string choice = null;
    while (choice != "h")
    {
        Console.WriteLine(@"Choose an option:
        a. Display all plants
        b. Post a plant for adoption
        c. Adopt a plant
        d. Delist a plant
        e. Plant of the Day!
        f. Search plants by light needs
        g. View stats
        h. Exit");

        choice = Console.ReadLine();
        if (choice == "a")
        {
            DisplayPlants();
        }
        else if (choice == "b")
        {
            AddAPlant();
        }
        else if (choice == "c")
        {
            AdoptAPlant();
        }
        else if (choice == "d")
        {
            DelistAPlant();
        }
        else if (choice == "e")
        {
            PlantOfTheDay();
        }
        else if (choice == "f")
        {
            SearchPlants();
        }
        else if (choice == "g")
        {
            ViewStats();
        }
        else if (choice == "h")
        {
            Console.WriteLine("Goodbye!");
        }
        else
        {
            Console.WriteLine("Please enter a valid menu choice");
        }

    }
}