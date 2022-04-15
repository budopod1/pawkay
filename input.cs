using System;
// using System.Collections;
using System.Collections.Generic;


class Input {
    public static int Option(string[] options) {
        while (true) {
            int i = 0;
            foreach (string option in options) {
                Console.WriteLine($"[{i}] {option}");
                i++;
            }
            if (int.TryParse(Console.ReadLine(), out int num)) {
                return num;
            } else {
                Console.WriteLine("Invalid option");
            }
        }
    }

    public static string PromptDefault(string otherwise) {
        string output = Console.ReadLine();
        if (output.Length > 0) {
            return output;
        }
        return otherwise;
    }
}
