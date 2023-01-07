using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;

namespace App
{
    partial class Client
    {
        private static readonly Dictionary<Type, Form> t_screens = new();

        public class Screen<T> where T : Form
        {
            public static T Instance
            {
                get
                {
                    try
                    {
                        if (t_screens.ContainsKey(typeof(T)))
                        {
                            return (T)Convert.ChangeType(t_screens[typeof(T)], typeof(T));
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                }
                set
                {
                    try
                    {
                        t_screens[typeof(T)] = value;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao registrar formulário {typeof(T)}. Exc.: {e}");
                    }
                }
            }

            public static void Show(params object[] parameters)
            {
                Screen.Show<T>(parameters);
            }

            public static void Close()
            {
                Screen.Close<T>();
            }

            public static void Hide()
            {
                Screen.Hide<T>();
            }

            public static void Update(params object[] parameters)
            {
                Screen.Update<T>(parameters);
            }
        }

        public class Screen
        {
            public static void Show<T>(params object[] parameters)
            {
                Show(typeof(T), parameters);
            }

            public static void Show(Type type, params object[] parameters)
            {
                MethodInvoker mth = delegate ()
                {
                    try
                    {
                        if (!t_screens.ContainsKey(type) ||
                            t_screens[type] == null ||
                            t_screens[type].IsDisposed)
                        {
                            t_screens[type] = (Form)Activator.CreateInstance(type);
                        }

                        Form screen = t_screens[type];

                        if (screen.Visible)
                        {
                            screen.Focus();
                            screen.BringToFront();
                        }
                        else
                        {
                            screen.TopMost = true;
                            screen.Show();
                        }

                        if (parameters.Length > 0)
                        {
                            Update(type, parameters);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao abrir formulário {type}. Exc.: {e}");
                    }
                };

                if (s_frmRoot.IsHandleCreated)
                {
                    s_frmRoot.Invoke(mth);
                }
                else
                {
                    mth.Invoke();
                }
            }

            public static void Close<T>()
            {
                MethodInvoker mth = delegate ()
                {
                    try
                    {
                        if (t_screens.ContainsKey(typeof(T)) && (t_screens[typeof(T)] != null))
                        {
                            t_screens[typeof(T)].Close();
                            t_screens.Remove(typeof(T));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao fechar formulário {typeof(T)}. Exc.: {e}");
                    }
                };

                if (s_frmRoot.IsHandleCreated)
                {
                    s_frmRoot.Invoke(mth);
                }
                else
                {
                    mth.Invoke();
                }
            }

            public static void Hide<T>()
            {
                MethodInvoker mth = delegate ()
                {
                    try
                    {
                        if (t_screens.ContainsKey(typeof(T)) && (t_screens[typeof(T)] != null))
                        {
                            if (t_screens[typeof(T)].Visible)
                            {
                                t_screens[typeof(T)].Hide();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao esconder formulário {typeof(T)}. Exc.: {e}");
                    }
                };

                if (s_frmRoot.IsHandleCreated)
                {
                    s_frmRoot.Invoke(mth);
                }
                else
                {
                    mth.Invoke();
                }
            }

            public static void Update<T>(params object[] parameters)
            {
                Update(typeof(T), parameters);
            }

            public static void Update(Type type, params object[] parameters)
            {
                try
                {
                    if (!t_screens.ContainsKey(type) || (t_screens[type] == null))
                    {
                        t_screens[type] = (Form)Activator.CreateInstance(type);
                    }

                    Type[] paramTypes = new Type[parameters.Length];
                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        paramTypes[i] = parameters[i].GetType();
                    }

                    MethodInfo method = paramTypes.Length > 0 ? type.GetMethod("Update", paramTypes) : null;

                    if (method != null)
                    {
                        method.Invoke(t_screens[type], parameters);
                    }
                    else
                    {
                        throw new Exception("Erro ao executar método \"Update\", pois o método não está implementado.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao atualizar formulário {type}. Exc.: {e}");
                }
            }
        }
    }
}
