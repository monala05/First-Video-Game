package learning;

import javax.swing.*;
import java.awt.*;

public class panel extends Canvas{

    private static final long serialVersionUID = -1841491879708122236L;

    public panel(int width, int height, String title, game Game){
        JFrame frame = new JFrame(title);

        frame.setPreferredSize(new Dimension(width, height));
        frame.setMinimumSize(new Dimension(width, height));
        frame.setMaximumSize(new Dimension(width, height));

        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setResizable(false);
        frame.setLocationRelativeTo(null);
        frame.add(Game);
        frame.setVisible(true);
        Game.start();
    }

}
