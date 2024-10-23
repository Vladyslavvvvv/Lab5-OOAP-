using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab5_OOAP_
{
    // Клас Counter, який відповідає за підрахунок продажів
    class Counter
    {
        // Статична змінна, що зберігає єдиний екземпляр класу Counter
        static Counter instance = new Counter();

        // Поля для збереження кількості проданих продуктів у кожній категорії
        int foodCount, medicineCount, clothesCount;
        // Поле для збереження сумарного доходу
        double totalRevenue;

        // Шлях до файлу, в якому зберігається лог продажів
        string filePath = "C:\\Users\\7ZipService\\Desktop\\3 курс\\Методи та засоби ООАП\\1 семестр\\Лабораторна робота №5\\sales_log.txt";

        // Приватний конструктор, який видаляє файл, якщо він існує, при створенні об'єкта
        private Counter()
        {
            // Якщо файл існує, він видаляється
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        // Метод для отримання екземпляра класу
        public static Counter Instance()
        {
            return instance;
        }

        // Метод для обробки продажу продукту в залежності від категорії
        public void SellProduct(string category)
        {
            // Отримуємо випадкову базову ціну для продукту
            double price = GetRandomPrice();
            // Початкове значення фінальної ціни
            double finalPrice = price;

            // Залежно від категорії продукту збільшуємо фінальну ціну та збільшуємо лічильник
            switch (category)
            {
                case "Продукти харчування":
                    // Надбавка 5% для продуктів харчування
                    finalPrice = price * 1.05;
                    foodCount++; // Збільшення кількості проданих продуктів харчування
                    break;
                case "Ліки":
                    // Надбавка 10% для ліків
                    finalPrice = price * 1.10;
                    medicineCount++; // Збільшення кількості проданих ліків
                    break;
                case "Одяг":
                    // Надбавка 15% для одягу
                    finalPrice = price * 1.15;
                    clothesCount++; // Збільшення кількості проданого одягу
                    break;
            }

            // Додаємо фінальну ціну до сумарного доходу
            totalRevenue += finalPrice;

            // Записуємо інформацію про продаж у файл
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: Продаж {category}, Вартість: {finalPrice:C}"); // Форматування вартості у вигляді валюти
            }
        }

        // Метод для генерації випадкової базової ціни товару в межах від 50 до 200
        private double GetRandomPrice()
        {
            Random rand = new Random(); // Генератор випадкових чисел
            return rand.Next(50, 201); // Повертає випадкове число між 50 і 200
        }

        // Метод для отримання статистики продажів
        public string GetStatistics()
        {
            // Повертає рядок зі статистикою кількості продажів та сумарним доходом
            return $"Продукти харчування: {foodCount} продано\n" +
                   $"Ліки: {medicineCount} продано\n" +
                   $"Одяг: {clothesCount} продано\n" +
                   $"Сумарний дохід: {totalRevenue:C}";
        }
    }

    // Клас форми, яка містить елементи інтерфейсу для взаємодії з користувачем
    public partial class Form1 : Form
    {
        // Елементи інтерфейсу: мітка для відображення статистики, кнопки для кожної категорії
        private Label label;
        private Button btnFood, btnMedicine, btnClothes;
        // Поле для об'єкта класу Counter
        private Counter counter;

        // Конструктор форми
        public Form1()
        {
            InitializeComponent(); // Ініціалізація форми
            InitializeControls();  // Ініціалізація елементів керування
            counter = Counter.Instance(); // Отримання екземпляра класу Counter
        }

        // Метод для ініціалізації елементів керування
        private void InitializeControls()
        {
            // Створення мітки для відображення статистики
            label = new Label { Location = new Point(20, 20), Size = new Size(300, 150) };
            Controls.Add(label); // Додавання мітки на форму

            // Кнопка для продажу продуктів харчування
            btnFood = new Button { Text = "Продати продукти харчування", Location = new Point(20, 200), Size = new Size(200, 30) };
            // Подія на клік кнопки, що викликає метод для продажу продуктів
            btnFood.Click += (sender, e) => SellProduct("Продукти харчування");
            Controls.Add(btnFood); // Додавання кнопки на форму

            // Кнопка для продажу ліків
            btnMedicine = new Button { Text = "Продати ліки", Location = new Point(20, 240), Size = new Size(200, 30) };
            // Подія на клік кнопки, що викликає метод для продажу ліків
            btnMedicine.Click += (sender, e) => SellProduct("Ліки");
            Controls.Add(btnMedicine); // Додавання кнопки на форму

            // Кнопка для продажу одягу
            btnClothes = new Button { Text = "Продати одяг", Location = new Point(20, 280), Size = new Size(200, 30) };
            // Подія на клік кнопки, що викликає метод для продажу одягу
            btnClothes.Click += (sender, e) => SellProduct("Одяг");
            Controls.Add(btnClothes); // Додавання кнопки на форму
        }

        // Метод для обробки продажу товару
        private void SellProduct(string category)
        {
            // Викликаємо метод для продажу товару з переданою категорією
            counter.SellProduct(category);
            // Оновлюємо текст мітки зі статистикою
            label.Text = counter.GetStatistics();
        }
    }
}