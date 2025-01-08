using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace PinballConsole
{
    public class SplashWindow : Window
    {
        public SplashWindow()
        {
            Title = "IFPA";
            Width = Dim.Fill();
            Height = Dim.Fill();

            var label = new Label("        ',:'/¯/`:,                  _               °   ,:´'*:^-:´¯'`:·,         ‘                    ,.-:~:-.            \r\n       /:/_/::::/';'      ,·~:-·´::/:`:;_.-~^*/'^;   '/::::/::::::::::;¯'`*:^:-.,  ‘              /':::::::::'`,          \r\n      /:'     '`:/::;‘    /::::::_:/::::::::::::/:::'i  /·´'*^-·´¯'`^·,/::::::::::::'`:,            /;:-·~·-:;':::',         \r\n      ;         ';:';‘  /·~-·´     `:;_,:·-~^*'`^:;  '`,             ¯'`*^·-:;::::::'\\' ‘       ,'´          '`:;::`,       \r\n      |         'i::i  'i                            i/    '`·,                     '`·;:::i'‘      /                `;::\\      \r\n      ';        ;'::i   `·,                     ,.·´         '|       .,_             \\:'/'     ,'                   '`,::;    \r\n      'i        'i::i'      `;         ,-·~:^*'´/;           'i       'i:::'`·,          i/' ‘   i'       ,';´'`;         '\\:::', ‘\r\n       ;       'i::;'       ';        ;-· ~·-;/:/'           'i       'i::/:,:          /'    ,'        ;' /´:`';         ';:::'i‘\r\n       ';       i:/'        ';       ,.,      ,'/              ;      ,'.^*'´     _,.·´‘     ;        ;/:;::;:';         ',:::;\r\n        ';     ;/ °         ;      ';:/`'*'*´                 ';     ;/ '`*^*'´¯           'i        '´        `'         'i::'/\r\n         ';   / °           ';     ;/'                         \\    /                      ¦       '/`' *^~-·'´\\         ';'/'‚\r\n          `'´       °        \\    /                            '`^'´‘                      '`., .·´              `·.,_,.·´  ‚\r\n           ‘                   `'´             '                                                                             ");

            label.X = Pos.Center();
            label.Y = Pos.Center();
            Add(label);

            var button = new Button("Press any key to continue");
            // position button center of X but below the label
            button.X = Pos.Center();
            button.Y = Pos.Bottom(label) + 1;
            button.Clicked += () => Application.RequestStop();
            Add(button);
        }
    }
}
