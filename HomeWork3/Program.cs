using HomeWork3;

Gun<int> pistol = new Gun<int>("pistol", 42);

Gun<string> bigPistol = new Gun<string>("big pistol", "high");

Gun<double> shotgun = new Gun<double>("shotgun", 72.5);

Pirate<Gun<int>> jackTheSparrow = new Pirate<Gun<int>>(pistol);



pistol.TheParameters();
Console.WriteLine();

shotgun.TheParameters();
Console.WriteLine();

bigPistol.TheParameters();
Console.WriteLine();

var piratesWeapon = jackTheSparrow.Create("crafted gun", 5);

piratesWeapon.TheParameters();



