using System.Numerics;
using Newtonsoft.Json;

namespace TextRPG
{    
    public class Store
    {
        public const int SWORD = 1;
        public const int ARMOR = 2;
        // 상점에서 사고 팔고, 구매 여부
        public List<Item> Item {  get; set; }

        public Store() // 생성자
        {
            Item = new List<Item> 
            {
                // 아이템 추가
                new Item(1,"수련자의 갑옷     ",  "방어력 +3  | 수련에 도움을 주는 갑옷이다.\t\t\t",0 ,3,1000,ARMOR),
                new Item(2,"무쇠갑옷          ",  "방어력 +9  | 무쇠로 만들어져 튼튼한 갑옷입니다.\t\t\t",0 ,9,1500, ARMOR),
                new Item(3, "스파르타 갑옷     ",  "방어력 +15 | 스파트라의 전사가 사용했다는 전설의 갑옷이다.\t",0 ,15,3500, ARMOR),
                new Item(4,"낡은 검           ",  "공격력 +2  | 쉽게 볼 수 있는 낡은 검이다\t\t\t",2 ,0,600, SWORD),
                new Item(5,"청동 도끼         ",  "공격력 +5  | 어디선가 사용됐던거 같은 도끼이다.\t\t\t",5 ,0,1500, SWORD),
                new Item(6,"스파르타의 창     ",  "공격력 +7  | 스파르타의 전사들이 사용했다는 전설의 창이다.\t",7 ,0,3500, SWORD),
                new Item(7,"크고 우람한 무언가",  "공격력 +15 | 굉장합니다.\t\t\t\t\t",15 ,0,4000,SWORD),
                new Item(8,"작고 초라한 어떤것",  "공격력 +6  | ...\t\t\t\t\t\t",6 ,0,600,SWORD)

            };
        }

    } // 상점 클래스
    public class Player
    {
        // 레벨, 이름 직업, 공격력, 방어력, 체력, 골드 , 인벤토리, 무기공격력, 무기방어력, 경험치, 레벨업경험치
        public int level { get; set; }
        public string name { get; set; }
        public string Job {  get; set; }
        public float Att { get; set;}
        public int Def { get; set;}
        public int HP { get; set;}
        public float Gold { get; set;}
        public List<Item> Inventory { get; set; }
        public int WeaponAtt { get; set;}
        public int WeaponDef { get; set; }
        public int EXP { get; set;}
        public int LevelEXP { get; set;}

        // 생성자
        public Player()
        {
            name = "김경민";
            level = 1;
            Job = "전사";
            Att = 10f;
            Def = 5;
            HP = 100;
            Gold = 500;
            Inventory = new List<Item>();

            WeaponAtt = 0;
            WeaponDef = 0;
            EXP = 0;
            LevelEXP = 1;
            
        }
        // 여기서 장착, 해제 관리
        public void EuqipItem(Item item)
        {
            if(!item.IsEquip)
            {
                item.IsEquip = true;
                WeaponAtt += item.ItemAttack;
                WeaponDef += item.ItemDefense;
            }
        }
        public void UnEuqipItem(Item item)
        {
            if(item.IsEquip)
            {
                item.IsEquip = false;
                WeaponAtt -= item.ItemAttack;
                WeaponDef -= item.ItemDefense;
            }
        }
        // 레벨업
        public void LevelUp()
        {
            if(LevelEXP == EXP)
            {
                level++;
                EXP = 0;
                LevelEXP++;
                Att += 0.5f;
                Def += 1;
            }
        }
        

    }// 플레이어 클래스
    public class Item // 아이템 클래스
    {
        // 식별번호,이름, 스텟(공격력, 방어력), 설명, 가격, 구매 여부, 장착 여부, 타입
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Tooltip { get; set; }
        public int ItemAttack { get; set; }
        public int ItemDefense { get; set; }
        public int Price { get; set; }
        public bool IsPruchase { get; set; }

        public bool IsEquip {  get; set; }

        public int type { get; set; } // 무기랑 방어구랑 나눠야되니깐

        public Item(int id, string itemName, string tooltip, int itemAttack, int itemDefense, int price,int type) // 생성자 
        {
            Id = id;
            ItemName = itemName;
            Tooltip = tooltip;
            ItemAttack = itemAttack;
            ItemDefense = itemDefense;
            Price = price;
            IsPruchase = false;
            IsEquip = false;
            this.type = type;
        }
    }
    internal class Program
    {
        public static Store EquipStore = new Store(); // 전역 생성

        static public void Rest(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("휴식하기");
                Console.WriteLine($"500 G 를 내면 체력을 회복할수 있습니다. (보유 골드 : {player.Gold} G)\n"); 
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    // 1번 선택시 500골드가 있으면 체력 100까지 회복 없으면 텍스트 출력 0 입력 시 메인메뉴로
                    if(input == 1)
                    {
                        if(player.Gold >= 500)
                        {
                            player.HP = 100; 
                            player.Gold -= 500;
                            Console.WriteLine("휴식을 완료했습니다.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다.");
                            Console.ReadKey();
                        }
                    }
                    if(input ==0)
                    {
                        mainMenu(player);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }

            }
        }

        static public void SellItem(Player player)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine("\n[아이템 목록]\n")


                for (int i = 0; i < player.Inventory.Count; i++) // 아이템 표시
                {
                    Console.WriteLine($"- {player.Inventory[i].ItemName} | {player.Inventory[i].Tooltip} | {(player.Inventory[i].Price) * 0.85}");
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int Input))
                {

                    if (Input == 0)
                    {
                        Store(player);
                    }
                    // 내 "인벤토리"에서 1번을 누르면 0번이 팔려야 되고 아이템의 85%의 가격을 내 골드에 더한다
                    // 여기에 더해서 장착하고 있는 아이템이면 장착 해제
                    
                    if (Input > 0 && Input <= player.Inventory.Count)
                    {
                        Item sellItem = player.Inventory[Input - 1];
                        sellItem.IsPruchase = false;
                        player.UnEuqipItem(sellItem); 
                        player.Inventory.Remove(sellItem);
                        player.Gold += (int)(sellItem.Price * 0.85f);
                    }
                    else
                    {
                        Console.WriteLine("\n판매할 수 없는 아이템입니다.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }

                

                
            }

        }
        static public void BuyItem(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G\n");

                Console.WriteLine("[아이템 목록]\n");
                for (int i = 0; i < EquipStore.Item.Count; i++)
                {
                    Console.WriteLine($"- {EquipStore.Item[i].Id} {EquipStore.Item[i].ItemName}| {EquipStore.Item[i].Tooltip} | {(EquipStore.Item[i].IsPruchase ? "구매 완료" : EquipStore.Item[i].Price)}"); 
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    if (Input > 0 && Input <= EquipStore.Item.Count)
                    {
                        Item buyitem = EquipStore.Item[Input - 1]; // 1번을 살 때 0 번을 사기 때문에
                        // 이미 산 거면 이미 샀다고 표시
                        if (buyitem.IsPruchase)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다");
                        }
                        else if (player.Gold >= buyitem.Price) // 사지 않았다면 골드 여부 체크, 골드가 있다면 구매, 플래그 변경
                        {
                            player.Gold -= buyitem.Price;
                            buyitem.IsPruchase = true;
                            player.Inventory.Add(buyitem);
                            Console.WriteLine("구매를 완료했습니다.");
                        }
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다.");
                        }
                    }
                    if (Input == 0)
                    {
                        Store(player); // 이전 메뉴
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    Console.ReadKey();
                }
                
            }
        }
      
        static public void Store(Player player)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine("\n[아이템 목록]\n");


            for (int i = 0; i < EquipStore.Item.Count; i++)
            {
                Console.WriteLine($"- {EquipStore.Item[i].Id} {EquipStore.Item[i].ItemName}| {EquipStore.Item[i].Tooltip} " +
                    $"| {(EquipStore.Item[i].IsPruchase ? "구매 완료" : EquipStore.Item[i].Price)}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    if (Input == 0)
                    {
                        mainMenu(player);
                    }
                    if (Input == 1)
                    {
                        BuyItem(player);
                    }
                    if (Input == 2)
                    {
                        SellItem(player);

                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    Console.ReadKey();
                }
            }



        }

        static public void EquipManager(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");

                for (int i = 0; i < player.Inventory.Count; i++) // 아이템 표시
                {
                    Console.WriteLine($"- {(player.Inventory[i].IsEquip ? "[E]" : " ")} " +
                        $"{i+1} {player.Inventory[i].ItemName} | {player.Inventory[i].Tooltip} ");
                    // 맨앞에 [E] 판정문
                }

                Console.WriteLine("\n0. 나가기");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    int temp = Input - 1;

                    if (temp >= 0 && temp < player.Inventory.Count)
                    {
                        Item selectItem = player.Inventory[temp];
                        // 장착 정보는 플레이어가 가지고 있을 테니깐 플레이어 클래스에 장착 여부 판정 메서드 추가

                        for (int i = 0; i < player.Inventory.Count; i++) // 그냥 다돌아서
                        {
                            if (player.Inventory[i].type == selectItem.type && !selectItem.IsEquip) // 타입이 같고, 끼고 있으면
                            {
                                player.UnEuqipItem(player.Inventory[i]); // 내 인벤토리에 있는 아이탬을 전체 장착해제.
                            }
                        }

                        if(!selectItem.IsEquip)
                        {
                            player.EuqipItem(selectItem);
                            Console.WriteLine("장착 성공");
                            Console.ReadKey();
                        }
                        else
                        {
                            player.UnEuqipItem(selectItem);
                            Console.WriteLine("해제 성공");
                                Console.ReadKey();
                        }
                    }

                    if (Input == 0)
                    {
                        Inventory(player);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    Console.ReadKey();
                }
            }


        }
       
        static public void Inventory(Player player)
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]\n");

            for (int i = 0; i < player.Inventory.Count; i++) // 아이템 표시
            {
                Console.WriteLine($"- {(player.Inventory[i].IsEquip ? "[E]" : " ")} " +
                    $"{i + 1} {player.Inventory[i].ItemName} | {player.Inventory[i].Tooltip} ");
                // 맨앞에 [E] 판정문
            }
            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");

            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    if (Input == 0)
                    {
                        mainMenu(player);
                    }
                    if (Input == 1)
                    {
                        EquipManager(player);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    Console.ReadKey();
                }
            }
        }

        static public void PlayerInfo(Player player)
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine($"Lv. {player.level}");
            Console.WriteLine($"Chad ( {player.Job}" + " )");
            Console.WriteLine($"공격력 : {player.Att + player.WeaponAtt} (+{player.WeaponAtt})"); // 캐릭의 기본 공격력 + 아이탬 공격력 
            Console.WriteLine($"방어력 : {player.Def + player.WeaponDef} (+{player.WeaponDef})");
            Console.WriteLine($"체 력 : {player.HP}");
            Console.WriteLine($"Gold : {player.Gold}" + " G\n");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    if (Input == 0)
                    {
                        mainMenu(player);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    Console.ReadKey();
                }
            }


        }

        static public void DungeonMakger(Player player, int RecommendDef, int min, int max)
        {
            Random rand = new Random();
            int HPrandNumber = rand.Next(20, 36); // 20 ~ 35 랜덤 값 생성
            float AddGold = rand.Next(min, max) * (player.Att + player.WeaponAtt) * 2f; // 받아온 min, max값 차이 랜덤 값
            int FailProbability = rand.Next(0, 10); // 던전 실패 확률

            int beforeHP = player.HP;
            float beforeGold = player.Gold;

            if (player.Def < RecommendDef && FailProbability < 4)
            {
                Console.Clear();
                Console.WriteLine("던전 실패");
                player.HP -= HPrandNumber + player.Def;
                Console.WriteLine($"체력 {beforeHP} -> {player.HP} ");

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

            }
            else
            {
                if((player.Def+player.WeaponDef) < HPrandNumber)
                {
                    player.HP -= HPrandNumber - (player.Def + player.WeaponDef); // 방어력이 던전 데미지를 웃돌땐 체력이 안까지게 하기 위함
                }
                player.Gold += 1000f + AddGold;

                player.EXP += 1;
                player.LevelUp(); // 레벨업 판정

                Console.Clear();
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!");

                if(RecommendDef == 5)
                    Console.WriteLine("쉬운 던전을 클리어 하셨습니다.\n");
                else if(RecommendDef == 11)
                    Console.WriteLine("일반 던전을 클리어 하셨습니다.\n");
                else if (RecommendDef == 17)
                    Console.WriteLine("어려운 던전을 클리어 하셨습니다.\n"); // 더 좋은 방법이 있을거 같은디..


                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {beforeHP} -> {player.HP} ");
                Console.WriteLine($"Gold {beforeGold} G -> {player.Gold} G");
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

            }

            if (int.TryParse(Console.ReadLine(), out int Input))
            {
                if (Input == 0)
                {
                    Dungeon(player);
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.\n");
                Console.ReadKey();
            }
        }

        static public void Dungeon(Player player)
        {
            Console.Clear();
            Console.WriteLine("던전입장\n");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 쉬운 던전 \t\t 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전 \t\t 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전 \t\t 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");

            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
             

            while (true)
            {

                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    switch (Input)
                    {
                        case 0:
                            mainMenu(player);
                            break;
                        case 1:
                            DungeonMakger(player, 5, 10, 20);
                            break;
                        case 2:
                            DungeonMakger(player, 11, 15, 25);
                            break;
                        case 3:
                            DungeonMakger(player, 17, 20, 30);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    Console.ReadKey();
                }
            }

        }

        static void mainMenu(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                if (int.TryParse(Console.ReadLine(), out int Input))
                {
                    Console.WriteLine();
                    switch (Input)
                    {
                        case 1:
                            PlayerInfo(player);
                            break;

                        case 2:
                            Inventory(player);
                            break;

                        case 3:
                            Store(player);
                            break;
                        case 4:
                            Dungeon(player);
                            break;
                        case 5:
                            Rest(player);
                            break;
                    }
                    if (Input >= 1 && Input <= 5)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.\n");
                        Console.ReadKey();
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            //Console.WriteLine("원하시는 이름을 설정해주세요.");
            //string PlayerName = Console.ReadLine();

            Store gameStore = new Store();
            Player player = new Player();
            mainMenu(player);

        }
    }
}
