

using System.Text.RegularExpressions;

namespace Tree
{


    class NodeOperationPair
    {
        public bool HasValues { get; set; }
        public Node? Uncle { get; set; }
        public RBOperation Operation { get; set; }

        public (Node, RBOperation) GetValues()
        {
            if (HasValues)
            {
                return (Uncle, Operation);
            }
            else
            {
                return (null, RBOperation.LeftNode);
            }
        }
    }
    enum Color
    {
        Red,
        Black
    };


    enum RBOperation
    {
        LeftNode,
        RightNode,
    };

    enum Rotation
    {
        Left,
        Right
    };

    class Node
    {
        public Color Color { get; set; }
        public IoTDevice Device { get; set; }

        public Node? Parent;

        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(IoTDevice device)
        {
            this.Color = Color.Red;
            this.Device = device;
        }


    }
    public class RedBlackTree
    {
        Node? root;
        long Count { get { return count; } }
        long count;
        static private RBOperation Check(IoTDevice a, IoTDevice b)
        {
            if (a.NumericalID <= b.NumericalID)
            {
                return RBOperation.LeftNode;
            }
            else
            {
                return RBOperation.RightNode;
            }
        }

        public void Add(IoTDevice device)
        {
            count++;
            root = AddRecursive(root, device);
        }

        private static Node AddRecursive(Node? node, IoTDevice device)
        {
            if (node == null)
            {
                return new Node(device);
            }
            var currentDevice = node.Device;
            var operationToExecute = Check(currentDevice, device);

            if (operationToExecute == RBOperation.LeftNode)
            {
                return node.Left = AddRecursive(node.Left, device);
            }
            else
            {
                return node.Right = AddRecursive(node.Right, device);
            }
        }

        public bool IsValidRedBlackTree()
        {
            var (redRed, minBlackHeight, maxBlackHeight) = ValidateRecursive(root, Color.Red, 0);
            return redRed == 0 && minBlackHeight == maxBlackHeight;
        }

        private static (long redRed, long minBlackHeight, long maxBlackHeight)
            ValidateRecursive(Node? node, Color parentColor, long blackHeight)
        {
            if (node != null)
            {
                long redRed = parentColor == Color.Red && node.Color == Color.Red ? 1 : 0;
                long newBlackHeight = blackHeight + (node.Color == Color.Black ? 1 : 0);

                var leftValidation = ValidateRecursive(node.Left, node.Color, newBlackHeight);
                var rightValidation = ValidateRecursive(node.Right, node.Color, newBlackHeight);

                return (
                    redRed + leftValidation.redRed + rightValidation.redRed,
                    Math.Min(leftValidation.minBlackHeight, rightValidation.minBlackHeight),
                    Math.Max(leftValidation.maxBlackHeight, rightValidation.maxBlackHeight)
                );
            }
            else
            {
                return (0, blackHeight, blackHeight);
            }
        }

        private static Color ParentColor(Node node)
        {
            return node!.Parent!.Color;
        }

        private static NodeOperationPair Uncle(Node node)
        {
            var parent = node.Parent;
            if (parent != null)
            {
                Node? grandparent = parent.Parent;
                if (grandparent != null)
                {
                    var operation = Check(grandparent.Device, parent.Device);
                    if (operation == RBOperation.LeftNode)
                    {
                        return new NodeOperationPair
                        {
                            HasValues = true,
                            Uncle = node,
                            Operation = RBOperation.RightNode
                        };
                    }
                    else
                    {
                        return new NodeOperationPair
                        {
                            HasValues = true,
                            Uncle = node,
                            Operation = RBOperation.LeftNode
                        };
                    }
                }
                else
                {
                    return new NodeOperationPair { HasValues = false };
                }
            }
            else
            {
                return new NodeOperationPair { HasValues = false };
            }
        }

        private Node FixTree(Node inserted)
        {
            bool notRoot = inserted.Parent == null;
            Node? root = null;
            if (notRoot)
            {
                bool parentIsRed = ParentColor(inserted) == Color.Red;
                Node? cursor = inserted;

                while (parentIsRed && notRoot)
                {
                    var uncleOperation = Uncle(cursor);

                    if (uncleOperation.HasValues)
                    {
                        var (uncle, operation) = uncleOperation.GetValues();

                        if (operation == RBOperation.LeftNode)
                        {
                            var parent = cursor.Parent;
                            if (uncle != null && uncle.Color == Color.Red)
                            {
                                parent.Color = Color.Black;
                                uncle.Color = Color.Black;

                                cursor = parent;
                            }
                            else
                            {
                                var parentSonOperation = Check(parent!.Device, cursor.Device);
                                if (parentSonOperation == RBOperation.LeftNode)
                                {

                                    var tmp = cursor.Parent;
                                    cursor = tmp;
                                    //HERE
                                    Rotate(cursor!, Rotation.Right);
                                    parent = cursor!.Parent;
                                }

                                parent!.Color  = Color.Black;
                                parent.Parent!.Color = Color.Red;
                                parent!.Parent!.Color = Color.Red;

                                var grandparent = cursor!.Parent!.Parent!;

                                Rotate(grandparent, Rotation.Left);
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                root = inserted;
            }

            inserted.Color = Color.Black;
            return inserted;
        }

        private void Rotate(Node node, Rotation direction)
        {
            if (direction == Rotation.Right)
            {
                var x = node;
                var y = node.Left;

                x.Left = y!.Right;


                y!.Parent = x.Parent;
                if (y.Right != null)
                {
                    y.Right.Parent = x;
                }


                var parent = x.Parent;
                if (parent != null)
                {
                    var insertDirection = Check(parent.Device, x.Device);
                    var isParentGreater = insertDirection == RBOperation.RightNode;
                    if (isParentGreater)
                    {
                        parent.Right = y;
                    }
                    else
                    {
                        parent.Left = y;
                    }
                }
                else
                {
                    y!.Parent = null;
                }

                y!.Right = x;
                x.Parent = y;

                y!.Right = x;
                x!.Parent = y;

            }
            else
            {
                var x = node;
                var y = node.Left;

                x.Right = y!.Left;

                y.Parent = x.Parent;

                if (y.Left != null)
                {
                    y.Left.Parent = x;
                }

                var parent = x.Parent;
                if (parent != null)
                {
                    var insertDirection = Check(parent.Device, x.Device);
                    var isParentLess = insertDirection == RBOperation.LeftNode;
                    if (isParentLess)
                    {
                        parent.Left = y;
                    }
                    else
                    {
                        parent.Right = y;
                    }
                }
                else
                {
                    y.Parent = null;
                }

                y.Left = x;
                x.Parent = y;
            }
        }

    }


}