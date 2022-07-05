﻿///  Mech 540-A  :  Team-2 Project; Path finding using RRT Algorithm
///
///  Name        :  Ajay Nalla
///  Student ID  :  49640014
///  Source file :  MainWindow.xaml.cs
///  Purpose     :  To create a GUI that can take user inputs for Start, End, Obstacles and show the generated path. 
///                 Contains UI-Layout namespace.
///  Description :  Contains classes 
///                 MainWindow: Inherits from the Window class, Which is a form. Contains methods to activate button
///                 click, mouse click, mouse move, mouse down and mouse up for various buttons and draw area.
///

/// ************************* USINGS *************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;
using PathPlanning;

namespace UI_Layout
{
    /// ************************* CLASSES *************************
    /// Class       : MainWindow
    /// Description : Inherits from Window class,Contains methods to activate button
    ///               click, mouse click, mouse move, mouse down and mouse up for 
    ///               various buttons and draw area.
    public partial class MainWindow : Window
    {
        /// ************************* GLOBAL VARIABLES *************************

        private UIElement _currentItem = null;
        private Point anchorPoint;

        private bool _isDrawingStart = false;
        private bool _isDrawingEnd = false;
        private bool _isDrawingRectangle = false;
        private bool _isRemoving = false;

        private bool _isDragingRectangleSize = false;

        public Point? StartPoint = null;
        public Point? EndPoint = null;

        /// ************************* METHOD *************************
        /// Method    : MainWindow
        /// Arguments : None
        /// Returns   : Nothing
        /// This method contains the component initializer, the program starts as soon as the window is open.
        public MainWindow()
        {
            InitializeComponent();
        }
       
        /// ************************* METHOD *************************
        /// Method    : CancelDrawing()
        /// Arguments : None
        /// Returns   : Nothing
        /// This method sets the Drawing elements to false, used for calling in subsequent methods.
        private void CancelDrawing()
        {
            _isDrawingStart = false;
            _isDrawingEnd = false;
            _isDrawingRectangle = false;

        }

        /// ************************* METHOD *************************
        /// Method    : StartButton_Click
        /// Arguments : object sender, RoutedEventArgs e
        /// Returns   : Nothing
        /// This method is a start point button click event, when the start button 
        /// is clicked the start point drawing is enabled.
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            CancelDrawing();
            _isDrawingStart = true;
        }

        /// ************************* METHOD *************************
        /// Method    : EndButton_Click
        /// Arguments : object sender, RoutedEventArgs e
        /// Returns   : Nothing
        /// This method is an end point button click event, when the end button is 
        /// clicked the end point drawing is enabled.
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            CancelDrawing();
            _isDrawingEnd = true;
        }

        /// ************************* METHOD *************************
        /// Method    : DrawRectangleButton_Click
        /// Arguments : object sender, RoutedEventArgs e
        /// Returns   : Nothing
        /// This method is a draw rectangle button click event, when the button is 
        /// clicked the drawing rectangle is enabled.
        private void DrawRectangleButton_Click(object sender, RoutedEventArgs e)
        {
            CancelDrawing();
            _isDrawingRectangle = true;
        }

        /// ************************* METHOD *************************
        /// Method    : DrawArea_OnPreviewMouseLeftButtonDown
        /// Arguments : object sender, MouseButtonEventArgs e
        /// Returns   : Nothing
        /// This method is for mouse leftbutton down with in the draw area 
        /// window, when mouse is clicked in the draw area, start point and end point
        /// are generated as Green and Red rectangles and obstacles are generated as
        /// blue rectangles.
        private void DrawArea_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //If start button is clicked and there is no start point already
            //specified in the draw area.
            if (_isDrawingStart && StartPoint == null)
            {
                //Create a new rectangle object.
                _currentItem = new Rectangle()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Green),
                    Tag = "Start"
                };

                //Getting x and y co-ordinates of the object.
                Canvas.SetLeft(_currentItem, e.GetPosition(DrawArea).X);
                Canvas.SetTop(_currentItem, e.GetPosition(DrawArea).Y);

                //Assiging the object as a StartPoint.
                StartPoint = new Point(e.GetPosition(DrawArea).X, e.GetPosition(DrawArea).Y);
                DrawArea.Children.Add(_currentItem);
                CancelDrawing();
            }

            //if end button clicked and there is no end point already in the draw area.
            if (_isDrawingEnd && EndPoint == null)
            {
                //create a rectangle object.
                _currentItem = new Rectangle()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Red),
                    Tag = "End"
                };

                //Getting the x and y co-ordinates of the object.
                Canvas.SetLeft(_currentItem, e.GetPosition(DrawArea).X);
                Canvas.SetTop(_currentItem, e.GetPosition(DrawArea).Y);

                //Assigning the object as EndPoint
                EndPoint = new Point(e.GetPosition(DrawArea).X, e.GetPosition(DrawArea).Y);
                DrawArea.Children.Add(_currentItem);
                CancelDrawing();
            }

            //if rectangle button is clicked
            if (_isDrawingRectangle)
            {
                //Capturing mouse movements in the draw area.
                DrawArea.CaptureMouse();
                anchorPoint = e.MouseDevice.GetPosition(DrawArea);

                //Creating new rectangle object.
                _currentItem = new Rectangle()
                {
                    Fill = new SolidColorBrush(Colors.Blue),
                    Tag = "Obstacle"
                };

                DrawArea.Children.Add(_currentItem);
                CancelDrawing();
                _isDragingRectangleSize = true;
            }

            //If the draw area has any element in it, this code allows user to move those elements
            //aorund as he wants. He can move start point, end point and obstacles in the draw area.
            if (_currentItem != null)
            {
                _currentItem.PreviewMouseMove += (s, args) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed && _currentItem != null)
                    {
                        _currentItem = (UIElement)s;
                        DragDrop.DoDragDrop(_currentItem, _currentItem, DragDropEffects.Move);
                    }
                };

                //Code for removing elements from the draw area.
                _currentItem.PreviewMouseLeftButtonDown += (o, args) =>
                {
                    if (_isRemoving)
                    {
                        if (((Rectangle)o).Tag == "End")
                        {
                            EndPoint = null;
                        }

                        if (((Rectangle)o).Tag == "Start")
                        {
                            StartPoint = null;
                        }
                        _isRemoving = false;
                        DrawArea.Children.Remove((UIElement)o);
                    }
                };
            }
        }

        /// ************************* METHOD *************************
        /// Method    : DrawArea_OnPreviewMouseLeftButtonUp
        /// Arguments : object sender, MouseButtonEventArgs e
        /// Returns   : Nothing
        /// This method is for mouse leftbutton up with in the draw area 
        /// window, when mouseclick is released triggers the release mouse capture event.
        private void DrawArea_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Stops the rectangle painting in the draw area.
            DrawArea.ReleaseMouseCapture();
        }

        /// ************************* METHOD *************************
        /// Method    : DrawArea_OnDragOver
        /// Arguments : object sender, DragEventArgs e
        /// Returns   : Nothing
        /// This method is collecting co-ordinates when mouse is dragged in the draw area.
        /// Updating the co-ordinates of the start and end points if they are dragged to new place.
        private void DrawArea_OnDragOver(object sender, DragEventArgs e)
        {
            Canvas.SetLeft(_currentItem, e.GetPosition(DrawArea).X);
            Canvas.SetTop(_currentItem, e.GetPosition(DrawArea).Y);

            if (((Rectangle)_currentItem).Tag == "End")
            {
                EndPoint = new Point(e.GetPosition(DrawArea).X, e.GetPosition(DrawArea).Y);
            }

            if (((Rectangle)_currentItem).Tag == "Start")
            {
                StartPoint = new Point(e.GetPosition(DrawArea).X, e.GetPosition(DrawArea).Y);
            }
        }

        /// ************************* METHOD *************************
        /// Method    : ResetButton_Click
        /// Arguments : object sender, RoutedEventArgs e
        /// Returns   : Nothing
        /// This method is a Resetbutton click event, when the reset button is clicked, 
        /// all the elements in the draw area are removed and all values are set to null.
        /// Cleaning/resetting the entire draw area.
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _currentItem = null;
            CancelDrawing();
            StartPoint = null;
            EndPoint = null;
            DrawArea.Children.Clear();
        }

        /// ************************* METHOD *************************
        /// Method    : RemoveButton_Click
        /// Arguments : object sender, RoutedEventArgs e
        /// Returns   : Nothing
        /// This method is a Remove button click event, when the remove element button is clicked, 
        /// the selected element with mouse click will be removed from the draw area.
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _isRemoving = true;
        }

        /// ************************* METHOD *************************
        /// Method    : DrawArea_OnPreviewMouseMove
        /// Arguments : object sender, MouseEventArgs e
        /// Returns   : Nothing
        /// This method is for collecting the co-ordinates of the rectangles in the draw area drawn by dragging.
        private void DrawArea_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if mousecaptures inside draw area is false
            if (!DrawArea.IsMouseCaptured)
                return;

            //The rectangle button is clicked and mouse is clicked within the draw area
            if (_isDragingRectangleSize)
            {
                //Collecting co-ordinates of the rectangle and allocating them to heigth and width of rectangle.
                Point location = e.MouseDevice.GetPosition(DrawArea);

                double minX = Math.Min(location.X, anchorPoint.X);
                double minY = Math.Min(location.Y, anchorPoint.Y);
                double maxX = Math.Max(location.X, anchorPoint.X);
                double maxY = Math.Max(location.Y, anchorPoint.Y);

                Canvas.SetTop(_currentItem, minY);
                Canvas.SetLeft(_currentItem, minX);

                double height = maxY - minY;
                double width = maxX - minX;

                ((Rectangle)_currentItem).Height = Math.Abs(height);
                ((Rectangle)_currentItem).Width = Math.Abs(width);
            }
        }

        /// ************************* METHOD *************************
        /// Method    : getRectangleData()
        /// Arguments : None
        /// Returns   : data (list of obstacle co-ordinates)
        /// This method is for creating a list of co-ordinates of the obstacles.
        public List<RectangleData> getRectangleData()
        {
            var data = new List<RectangleData>();
            foreach (UIElement drawAreaChild in DrawArea.Children)
            {
                if (((Rectangle)drawAreaChild).Tag == "Obstacle")
                {
                    double x1 = Canvas.GetLeft(drawAreaChild);
                    double y1 = Canvas.GetTop(drawAreaChild);

                    double x2 = x1 + ((Rectangle)drawAreaChild).Width;
                    double y2 = y1;

                    double x3 = x1;
                    double y3 = y1 + ((Rectangle)drawAreaChild).Height;

                    double x4 = x1 + ((Rectangle)drawAreaChild).Width;
                    double y4 = y1 + ((Rectangle)drawAreaChild).Height; 

                    data.Add(new RectangleData(x1, y1, x2, y2,x3,y3,x4,y4));
                }  
            }
            return data;
        }

        /// ************************* METHOD *************************
        /// Method    : CreatePolyline
        /// Arguments : List of Nodes (points)
        /// Returns   : Nothing
        /// This method is for drawing circles on the canvas and connecting those
        /// circles with line. The path from start to end will be shown.
        public void CreatePolyline(List<Node> list_1)
        {
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            // Create a polyline with properties
            Polyline polyline = new Polyline();
            polyline.Stroke = blackBrush;
            polyline.StrokeThickness = 2;

            // Create a collection of points for a polyline  
            PointCollection polygonPoints = new PointCollection();
            foreach (Node node in list_1)
            {
                CreatePath(node);
                Point nodepoint = new Point(node.X+1, node.Y+1);
                polygonPoints.Add(nodepoint);
            }
  
            // Set Polyline.Points properties  
            polyline.Points = polygonPoints;
            // Add polyline to the draw area children
            DrawArea.Children.Add(polyline);
        }

        /// ************************* METHOD *************************
        /// Method    : CreatePath
        /// Arguments : Node
        /// Returns   : Nothing
        /// This method is for creating drawing circles on canvas at given nodes.
        public void CreatePath(Node node)
        {
            var circle1 = new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.Black),
                Width = 3,
                Height = 3
            };

            Canvas.SetLeft(circle1, node.X + 1);
            Canvas.SetTop(circle1, node.Y + 1);

            DrawArea.Children.Add(circle1);
        }
        /// ************************* METHOD *************************
        /// Method    : GeneratePathButton_Click
        /// Arguments : object sender, RoutedEventArgs e
        /// Returns   : Nothing
        /// This method is generate button click event, when this is clicked
        /// the start point and point co-ordinates are sent to console winodw. 
        /// Here the next code for collecting rectangle object co-ordinates also will be
        /// coming, the RRT algorith can be called/interacted within this button to take 
        /// co-ordinates of start, end and obstacle data ==> do the path generation ==>
        /// and send the co-ordinates of path points that the GUI can use to draw the line path.
        /// Calls multiple methods to achive the visual representation of Path generated by the Algorithm.
        private void GeneratePathButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartPoint == null || EndPoint == null)
            {
                return;
            }

            //Get the list of obstacles store in data
            List<RectangleData> data = getRectangleData();
            
            string message = null;
            message += "Start Point is" + "(" + " " + StartPoint.ToString() + " " + ")" + "\nEnd Point is" + "(" + " " + EndPoint.ToString() + " " + ")";
            MessageBox.Show(message, "You are good to go please click ok to continue");

            //Calling RRTree algorithm into the GUI
            RRTree tree = new RRTree((Point)StartPoint, (Point)EndPoint);
            List<Node> finalPath = tree.CreateAndSearchRRT(data);

            //Creating the path on the canvas from start to end
            CreatePolyline(finalPath);

            float dist_travelled = tree.ReturnFinalDistanceTravelled(finalPath);
            MessageBox.Show("Path Found! Distance Travelled is "+ dist_travelled.ToString(),"The End");
            ResetButton_Click(sender, e);

        }
        
    }
}
