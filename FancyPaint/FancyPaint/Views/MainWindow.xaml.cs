using Infragistics.Controls.Editors;
using Infragistics.Windows.Ribbon;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace FancyPaint.Views
{
    public partial class MainWindow : XamRibbonWindow
    {
        System.Drawing.Graphics g;
        private enum CurrShape { Line, Rectangle, Ellipse};
        CurrShape currShape = CurrShape.Line;
        System.Windows.Point start, end, currentPoint;
        public MainWindow()
        {
            InitializeComponent();
            canvas.EraserShape = new RectangleStylusShape(2.4, 2.4);
        }

        private void canvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(canvas);
            InkCanvas i = new InkCanvas();
            
            
        }

        private void canvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();

                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X - 50;
                line.Y1 = currentPoint.Y - 50;
                line.X2 = e.GetPosition(this).X - 50;
                line.Y2 = e.GetPosition(this).Y - 50;
                currentPoint = e.GetPosition(this);
                canvas.Children.Add(line);
            }
        }
        private void canvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Draw the correct shape
            switch (currShape)
            {
                case CurrShape.Line:
                    DrawLine();
                    break;

                case CurrShape.Ellipse:
                    DrawEllipse();
                    break;

                case CurrShape.Rectangle:
                    DrawRectangle();
                    break;

                default:
                    return;
            }
        }

        private void DrawLine()
        {
            Line newLine = new Line()
            {
                Stroke = Brushes.Blue,
                X1 = start.X - 50,
                Y1 = start.Y - 50,
                X2 = end.X - 50,
                Y2 = end.Y - 50 
            };
            canvas.Children.Add(newLine);
        }

        // Sets and draws ellipse after mouse is released
        private void DrawEllipse()
        {
            Ellipse newEllipse = new Ellipse()
            {
                Stroke = Brushes.Green,
                Fill = Brushes.Red,
                StrokeThickness = 4,
                Height = 10,
                Width = 10
            };

            // If the user the user tries to draw from
            // any direction then down and to the right
            // you'll get an error unless you use if
            // to change Left & TopProperty and Height
            // and Width accordingly

            if (end.X >= start.X)
            {
                // Defines the left part of the ellipse
                newEllipse.SetValue(Canvas.LeftProperty, start.X);
                newEllipse.Width = end.X - start.X;
            }
            else
            {
                newEllipse.SetValue(Canvas.LeftProperty, end.X);
                newEllipse.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {
                // Defines the top part of the ellipse
                newEllipse.SetValue(Canvas.TopProperty, start.Y - 50);
                newEllipse.Height = end.Y - start.Y;
            }
            else
            {
                newEllipse.SetValue(Canvas.TopProperty, end.Y - 50);
                newEllipse.Height = start.Y - end.Y;
            }

            canvas.Children.Add(newEllipse);
        }

        private void LineTool_Click(object sender, RoutedEventArgs e)
        {
            currShape = CurrShape.Line;
        }

        private void RectTool_Click(object sender, RoutedEventArgs e)
        {
            //currShape = CurrShape.Rectangle;
            Line line = new Line();
            line.X1 = 30;
            line.Y1 = 30;
            line.Height = 100;
            line.Width = 200;
            line.Fill = Brushes.Blue;
            canvas.Children.Add(line);
        }

        private void Erase_Click(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void ComboEditorTool_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            canvas.DefaultDrawingAttributes.Width = double.Parse(combo.SelectedItem.ToString());
            canvas.DefaultDrawingAttributes.Height = double.Parse(combo.SelectedItem.ToString());
        }

        private void Brush_Click(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            canvas.Paste();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            canvas.CutSelection();
            
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            canvas.CopySelection();
        }

        private void PenColor_Click(object sender, RoutedEventArgs e)
        {
            XamColorPicker MyColorPicker = new XamColorPicker();
            MyColorPicker.DerivedPalettesCount = 10;
            MyColorPicker.Width = 100;
            MyColorPicker.Height = 20;
            MyColorPicker.SelectedColor = Colors.Black;

        }

        private void MyColorPicker_DropDownClosed(object sender, System.EventArgs e)
        {
            canvas.DefaultDrawingAttributes.Color = (Color)MyColorPicker.SelectedColor;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "null";
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Title = "Save your picture";
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = saveFileDialog1.FileName;
                    myWindow.Title = "Tp 4" + saveFileDialog1.FileName;
                }
                FileStream inkFileStream = new FileStream($"{filePath}.ink", FileMode.Create);
                canvas.Strokes.Save(inkFileStream);
                inkFileStream.Close();
            }catch(Exception er)
            {
                MessageBox.Show(er.Message);
            }
            
            MessageBox.Show("Yes");
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = "";
                System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Title = "Open your picture";
                openFileDialog1.Filter = "Ink files (*.ink)|*.ink";
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = openFileDialog1.FileName;
                    myWindow.Title = "Tp 4" + openFileDialog1.FileName;
                }
                    

                // If our file exists,
                if (File.Exists(filePath))
                {
                    FileStream inkFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    StrokeCollection strokes = new StrokeCollection(inkFileStream);
                    inkFileStream.Close();
                    canvas.Strokes = strokes;
                }

            }catch(Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
            

            private void EllipTool_Click(object sender, RoutedEventArgs e)
        {
            currShape = CurrShape.Ellipse;
        }

        private void DrawRectangle()
        {
            Rectangle newRectangle = new Rectangle()
            {
                Stroke = Brushes.White,
                Fill = Brushes.LightBlue,
                StrokeThickness = 4
            };

            if (end.X >= start.X)
            {
                // Defines the left part of the rectangle
                newRectangle.SetValue(Canvas.LeftProperty, start.X);
                newRectangle.Width = end.X - start.X;
            }
            else
            {
                newRectangle.SetValue(Canvas.LeftProperty, end.X);
                newRectangle.Width = start.X - end.X;
            }

            if (end.Y >= start.Y)
            {
                // Defines the top part of the rectangle
                newRectangle.SetValue(Canvas.TopProperty, start.Y - 50);
                newRectangle.Height = end.Y - start.Y;
            }
            else
            {
                newRectangle.SetValue(Canvas.TopProperty, end.Y - 50);
                newRectangle.Height = start.Y - end.Y;
            }

            canvas.Children.Add(newRectangle);
        }
    }
}
