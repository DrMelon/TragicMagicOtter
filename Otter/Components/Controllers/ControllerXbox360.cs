namespace Otter {
    public class ControllerXbox360 : Controller {

        public static int JoyButtonA = 0;
        public static int JoyButtonB = 1;
        public static int JoyButtonX = 2;
        public static int JoyButtonY = 3;
        public static int JoyButtonLB = 4;
        public static int JoyButtonRB = 5;
        public static int JoyButtonBack = 6;
        public static int JoyButtonStart = 7;
        public static int JoyButtonLeftStick = 8;
        public static int JoyButtonRightStick = 9;

        public Button A { get { return Button(Controls.A); } }
        public Button B { get { return Button(Controls.B); } }
        public Button X { get { return Button(Controls.X); } }
        public Button Y { get { return Button(Controls.Y); } }
        public Button RB { get { return Button(Controls.RB); } }
        public Button LB { get { return Button(Controls.LB); } }
        public Button LeftStickClick { get { return Button(Controls.LStickClick); } }
        public Button RightStickClick { get { return Button(Controls.RStickClick); } }
        public Button Start { get { return Button(Controls.Start); } }
        public Button Back { get { return Button(Controls.Back); } }
        public Button RT { get { return Button(Controls.RT); } }
        public Button LT { get { return Button(Controls.LT); } }
        public Button Up { get { return Button(Controls.Up); } }
        public Button Down { get { return Button(Controls.Down); } }
        public Button Left { get { return Button(Controls.Left); } }
        public Button Right { get { return Button(Controls.Right); } }

        public Axis LeftStick { get { return Axis(Controls.LStick); } }
        public Axis RightStick { get { return Axis(Controls.RStick); } }
        public Axis DPad { get { return Axis(Controls.DPad); } }
        public Axis Triggers { get { return Axis(Controls.Triggers); } }

        public ControllerXbox360(params int[] joystickId) {
            AddButton(Controls.A);
            AddButton(Controls.B);
            AddButton(Controls.X);
            AddButton(Controls.Y);
            AddButton(Controls.RB);
            AddButton(Controls.LB);
            AddButton(Controls.LStickClick);
            AddButton(Controls.RStickClick);
            AddButton(Controls.Start);
            AddButton(Controls.Back);
            AddButton(Controls.RT);
            AddButton(Controls.LT);
            AddButton(Controls.Up);
            AddButton(Controls.Down);
            AddButton(Controls.Left);
            AddButton(Controls.Right);

            AddAxis(Controls.LStick);
            AddAxis(Controls.RStick);
            AddAxis(Controls.DPad);
            AddAxis(Controls.Triggers);

            foreach (var joy in joystickId) {
                A.AddButton(0, joy);
                B.AddButton(1, joy);
                X.AddButton(2, joy);
                Y.AddButton(3, joy);
                LB.AddButton(4, joy);
                RB.AddButton(5, joy);
                Back.AddButton(6, joy);
                Start.AddButton(7, joy);
                LeftStickClick.AddButton(8, joy);
                RightStickClick.AddButton(9, joy);

                RT.AddAxisButton(AxisButton.ZMinus, joy);
                LT.AddAxisButton(AxisButton.ZPlus, joy);

                LeftStick.AddAxis(JoyAxis.X, JoyAxis.Y, joy);
                RightStick.AddAxis(JoyAxis.U, JoyAxis.R, joy);
                DPad.AddAxis(JoyAxis.PovX, JoyAxis.PovY, joy);
                Triggers.AddAxis(JoyAxis.Z, JoyAxis.Z, joy);

                Up
                    .AddAxisButton(AxisButton.YMinus, joy)
                    .AddAxisButton(AxisButton.PovYMinus, joy);
                Down
                    .AddAxisButton(AxisButton.YPlus, joy)
                    .AddAxisButton(AxisButton.PovYPlus, joy);
                Right
                    .AddAxisButton(AxisButton.XPlus, joy)
                    .AddAxisButton(AxisButton.PovXPlus, joy);
                Left
                    .AddAxisButton(AxisButton.XMinus, joy)
                    .AddAxisButton(AxisButton.PovXMinus, joy);
            }
        }

        enum Controls {
            A,
            B,
            X,
            Y,
            RB,
            LB,
            RT,
            LT,
            LStickClick,
            RStickClick,
            LStick,
            RStick,
            Start,
            Back,
            DPad,
            Triggers,
            Up,
            Down,
            Left,
            Right
        }

    }

    
}
