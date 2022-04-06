using System;
// using System.Collections;
using System.Collections.Generic;


class Input {
    public static int Option(string[] options) {
        int i = 0;
        foreach (string option in options) {
            Console.WriteLine($"[{i}] {option}");
            i++;
        }
        return Int32.Parse(Console.ReadLine());
    }
}
