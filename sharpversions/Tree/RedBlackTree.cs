

namespace Tree
{

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

            public Node? Left {get; set;}
            public Node? Right{get; set;}

            public Node(IoTDevice device) {
                this.Color = Color.Red;
                this.Device = device;
            }


        }
    public class RedBlackTree
    {
        Node? root;
        long Count {get { return count; }}
        long count;
        static private RBOperation Check(IoTDevice a, IoTDevice b) {
            if (a.NumericalID <= b.NumericalID) {
                return RBOperation.LeftNode;
            } else {
                return RBOperation.RightNode;
            }
        }

        public void Add(IoTDevice device) {
            count++;
            root = AddRecursive(root, device);
        }

        private static Node AddRecursive(Node? node, IoTDevice device) {
            if (node == null) {
                return new Node(device);
            }
            var currentDevice = node.Device;
            var operationToExecute = Check(currentDevice, device);

            if (operationToExecute == RBOperation.LeftNode) {
                return AddRecursive(node.Left, device);
            } else {
                return AddRecursive(node.Right, device);
            }
        }

        public bool IsValidRedBlackTree() {
            var ( redRed, minBlackHeight, maxBlackHeigh) = ValidateRecursive(root, Color.Red, 0);
            return redRed == 0 && minBlackHeight == maxBlackHeight;
        }

        private static (long redRed, long minBlackHeight, long maxBlackHeight) 
            ValidateRecursive(Node? node, Color parentColor, long blackHeight) {
            if (node != null) {
                long redRed = parentColor == Color.Red && node.Color == Color.Red ? 1 : 0;
                long newBlackHeight = blackHeight + (node.Color == Color.Black ? 1 : 0);

                var leftValidation = ValidateRecursive(node.Left, node.Color, newBlackHeight);
                var rightValidation = ValidateRecursive(node.Right, node.Color, newBlackHeight);

                return (
                    redRed + leftValidation.redRed + rightValidation.redRed,
                    Math.Min(leftValidation.minBlackHeight, rightValidation.minBlackHeight),
                    Math.Max(leftValidation.maxBlackHeight, rightValidation.maxBlackHeight)
                );
            } else {
                return (0, blackHeight, blackHeight);
            }
        }
    }
}