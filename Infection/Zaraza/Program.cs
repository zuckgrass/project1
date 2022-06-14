using System;
using System.Threading;
using static System.Console;
using System.Collections;
using System.Collections.Generic;

namespace Zaraza
{
	struct Cell
	{
		private bool Infected;
		private bool Immune;
		private bool Healthy;
		private int Counter;
		public Cell(bool Infected, bool Immune, bool Healthy, int Counter)
		{
			this.Infected = Infected;
			this.Immune = Immune;
			this.Healthy = Healthy;
			this.Counter = Counter;
		}
		public bool getInfected() => Infected;
		public bool getImmune() => Immune;
		public bool getHealthy() => Healthy;
		public int getCounter() => Counter;
	}
	class PlayField
	{
		public List<List<Cell>> cells = new List<List<Cell>>();
		static Random rnd = new Random();
		public void createStartPlayField()
		{
			for (int i = 0; i < 52; i++)
			{
				cells.Add(new List<Cell>());
				for (int j = 0; j < 26; j++)
				{

					if (i == 24 && j == 12)
					{
						cells[i].Add(new Cell(true, false, false, 7));
					}
					else
					{
						cells[i].Add(new Cell(false, false, true, 0));
					}
				}
			}
		}

		public void updateCells()
		{
			for (int i = 0; i < 52; i++)
			{
				for (int j = 0; j < 26; j++)
				{
					var currentCell = cells[i][j];
					if (currentCell.getInfected() && currentCell.getCounter() < 7 && currentCell.getCounter() != 0)
					{
						infectCell(i + 1, j);
						infectCell(i, j + 1);
						infectCell(i + 1, j + 1);
						infectCell(i, j - 1);
						infectCell(i - 1, j);
						infectCell(i - 1, j - 1);
						infectCell(i + 1, j - 1);
						infectCell(i - 1, j + 1);
					}
					if (currentCell.getInfected() && currentCell.getCounter() == 0)
					{
						cells[i][j] = new Cell(false, true, false, 6);
					}
					else if (currentCell.getImmune() && currentCell.getCounter() == 0)
					{
						cells[i][j] = new Cell(false, false, true, 0);
					}
					else
					{
						cells[i][j] = new Cell(currentCell.getInfected(), currentCell.getImmune(), currentCell.getHealthy(), currentCell.getCounter() - 1);
					}
				}
			}
		}
		public void infectCell(int i, int j)
		{
			if (i < 0 || j < 0 || i > 51 || j > 25)
			{
				return;
			}
			var currentCell = cells[i][j];
			if (currentCell.getImmune() || currentCell.getInfected())
			{
				return;
			}
			var random = rnd.Next(0, 2);
			if (random == 1)
			{
				cells[i][j] = new Cell(true, false, false, 7);
			}
		}
	}
	class MainClass
	{
		public static void DisplayPlayField(List<List<Cell>> cells)
		{
			for (int i = 0; i < 52; i++)
			{
				for (int j = 0; j < 26; j++)
				{
					var currentCell = cells[i][j];
					Console.SetCursorPosition(i, j);
					if (currentCell.getInfected())
					{
						Console.BackgroundColor = ConsoleColor.Red;
					}
					else if (currentCell.getImmune())
					{
						Console.BackgroundColor = ConsoleColor.Blue;
					}
					else
					{
						Console.BackgroundColor = ConsoleColor.Green;
					}
					Write(" ");
					Console.BackgroundColor = ConsoleColor.Black;
				}
				WriteLine();
			}
		}
		static void Main()
		{
			var field = new PlayField();
			field.createStartPlayField();
			for (int k = 1; k <= 70; k++)
			{
				MainClass.DisplayPlayField(field.cells);
				field.updateCells();
				Thread.Sleep(1000);
			}
		}
	}
}