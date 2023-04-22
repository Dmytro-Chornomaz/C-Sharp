using System.Diagnostics.Metrics;

        Cleaner cleaner = new Cleaner();
        cleaner.TheCleaner();
        cleaner.TheCounter(Person.SalaryRate, Person.RobustRate, Person.VacationTerm, 
                           cleaner.ModSR, cleaner.ModRR, cleaner.ModVT);
        cleaner.Skill(cleaner.skill);
        cleaner.Request(cleaner.request);
        cleaner.FavoriteFruits();
        cleaner.WeightHandler(75);
        Console.WriteLine();

        Engineer engineer = new Engineer();
        engineer.TheEngineer();
        engineer.TheCounter(Person.SalaryRate, Person.RobustRate, Person.VacationTerm,
                           engineer.ModSR, engineer.ModRR, engineer.ModVT);
        engineer.Skill(engineer.skill);
        engineer.Request(engineer.request);
        engineer.FavoriteFruits();
        engineer.WeightHandler(82);
        Console.WriteLine();

        MiniBoss miniBoss = new MiniBoss();
        miniBoss.TheMiniBoss();
        miniBoss.TheCounter(Person.SalaryRate, Person.RobustRate, Person.VacationTerm,
                           miniBoss.ModSR, miniBoss.ModRR, miniBoss.ModVT);
        miniBoss.Bonus();
        miniBoss.Skill(miniBoss.skill);
        miniBoss.Request(miniBoss.request);
        miniBoss.FavoriteFruits();
        miniBoss.WeightHandler(100);
        Console.WriteLine();

        BigBoss bigBoss = new BigBoss();
        bigBoss.TheBigBoss();
        bigBoss.TheCounter(Person.SalaryRate, Person.RobustRate, Person.VacationTerm,
                           bigBoss.ModSR, bigBoss.ModRR, bigBoss.ModVT);
        bigBoss.Bonus();
        bigBoss.Skill(bigBoss.skill);
        bigBoss.Request(bigBoss.request);
        bigBoss.FavoriteFruits();
        bigBoss.WeightHandler(87);
        Console.WriteLine();

        LittleInfo.Salary();
        LittleInfo.Vacation();
       