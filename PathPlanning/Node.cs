﻿///  Mech 540-A  :  Team-2 Project; Path finding using RRT Algorithm
///
///  Name        :  Anna-Lee McLean
///  Student ID  :  81058794
///  Source file :  Node.cs
///  Purpose     :  Contains the Node class within the PathPlanning namespace. Allows a node within an RRT roadmap to be created 
///                 with (x,y) coordinates, it's parent node in the roadmap, and cost-to-goal distance.
///  Description :  Contains the EuclideanDistance() Method which calculates the straight-line distance between two nodes.
///                 X and Y coordinates for the node must be declared when the class is instantiated.
///                


/// ******************************* USINGS ********************************
using System;

namespace PathPlanning
{
    /// ***************************** CLASSES *****************************
    /// Class       : Node
    /// Description : Represents a node in the RRT roadmap which has an x and y coordinate, 
    ///               a parent node and a cost to be travelled.
    /// Methods     : 1. EuclideanDistance()
    /// 
    
    public class Node 
    {
        // ATTRIBUTES
        public double X;
        public double Y;
        
        // CONSTRUCTOR
        public Node(double X_, double Y_)
        {
            X = X_;
            Y = Y_;
            cost = 0;
        }

        // GETTERS AND SETTERS

        // Gets and Sets the node to which the instantiated node is connected to in the roadmap
        public Node parent { get; set; }
        // Gets and Sets the straight-line distance between the node and its parent
        public float cost { get; set; }
       
        /// ***************************** METHODS *****************************
        /// Method    : EuclideanDistance()
        /// Arguments : 1 (Node)
        /// Returns   : The resulting straight-line distance (float)
        
        public float EuclideanDistance(Node node2)
        {
            float distance = (float)Math.Sqrt(Math.Pow(X - node2.X, 2) + Math.Pow(Y - node2.Y, 2));
            return distance;
        }
    }
}
