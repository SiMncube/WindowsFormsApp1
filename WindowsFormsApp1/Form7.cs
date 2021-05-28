﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/mm";
            dateTimePicker1.ShowUpDown = true;
            customerTableAdapter1.Fill(fullDatabase1.Customer);
            paymentTableAdapter1.Fill(fullDatabase1.Payment);
            bookingSummaryTableAdapter1.Fill(fullDatabase1.BookingSummary);
            label7.Text += getAmountDue();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(paymentDetailIsValid())
            {
                paymentTableAdapter1.Insert(DateTime.Now, getAmountDue(), currentUser.getSummaryID(), "EFT");
                updateBookingStatus();
                updateBookedRoom();
                label8.Text += getAmountDue();
                label9.Visible = true;
                label8.Visible = true;
                label6.Visible = false;
                label7.Visible = false;
                resetColor(textBox1);
                resetColor(textBox2);
                resetColor(textBox3);
                paymentTableAdapter1.Fill(fullDatabase1.Payment);
                button1.Enabled = false;
            }
        }       
        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private string getSurname()
        {
            int startIndex = 0;
            for (int i = 0; i < textBox3.Text.Length; i++)
            {
                if (textBox3.Text[i] == ' ')
                {
                    startIndex = i + 1;
                    break;
                }
            }
            return textBox3.Text.Substring(startIndex);
        }
        private bool nameIsValid()
        {
            for (int i = 0; i < fullDatabase1.Customer.Rows.Count; i++)
            {
                if (fullDatabase1.Tables["Customer"].Rows[i]["emailID"].ToString() == currentUser.getEmailID() && fullDatabase1.Tables["Customer"].Rows[i]["surname"].ToString() == getSurname())
                    return true;
            }
            return false;
        }
        private bool cardNumberIsValid()
        {
            return textBox1.Text.Length == 16;
        }
        private bool cardNotExpired()
        {
            return dateTimePicker1.Value.Year > DateTime.Today.Year;
        }
        private bool cvvIsValid()
        {
            return textBox2.Text.Length == 3;
        }
        private bool paymentDetailIsValid()
        {
            int count = 0;
            if(!nameIsValid())
            {
                textBox3.BackColor = Color.Red;
                count++;
            }
            if(!cardNumberIsValid())
            {
                textBox1.BackColor = Color.Red;
                count++;
            }
            if(!cvvIsValid())
            {
                textBox2.BackColor = Color.Red;
                count++;
            }
            if(!cardNotExpired())
            {
                label6.Visible = true;
                count++;
            }
            return count == 0;
        }
        private string getAmountDue()
        {
            for (int i = 0; i < fullDatabase1.BookingSummary.Rows.Count; i++)
            {
                if(fullDatabase1.Tables["BookingSummary"].Rows[i]["summaryID"].ToString() == currentUser.getSummaryID()+"")
                    return fullDatabase1.Tables["BookingSummary"].Rows[i]["amountDue"].ToString();
            }
            return "";
        }
        private void updateBookingStatus()
        {
            for (int i = 0; i < fullDatabase1.BookingSummary.Rows.Count; i++)
            {
                if (fullDatabase1.Tables["BookingSummary"].Rows[i]["summaryID"].ToString() == currentUser.getSummaryID() + "")
                {
                    fullDatabase1.Tables["BookingSummary"].Rows[i]["bookingStatus"] = "Complete";
                    bookingSummaryTableAdapter1.Update(fullDatabase1.BookingSummary);
                    bookingSummaryTableAdapter1.Fill(fullDatabase1.BookingSummary);
                }

            }
        }
        private void updateBookedRoom()
        {
            int[] rooms = currentUser.getRoomIDs();
            for (int i = 0; i < rooms.Length; i++)
            {
                for (DateTime dateID = GetDateIn(); DateTime.Compare(dateID, GetDateOut()) < 0; dateID = dateID.AddDays(1))
                {
                    bookedRoomTableAdapter1.Insert(dateID, currentUser.getSummaryID(), rooms[i]);
                    bookingSummaryTableAdapter1.Fill(fullDatabase1.BookingSummary);
                }
            }
        }
        private DateTime GetDateIn()
        {
            DateTime dateIn =  DateTime.Now;
            for (int i = 0; i < fullDatabase1.BookingSummary.Rows.Count; i++)
            {
                if (fullDatabase1.Tables["BookingSummary"].Rows[i]["summaryID"].ToString() == currentUser.getSummaryID() + "")
                {
                    string dateString = fullDatabase1.Tables["BookingSummary"].Rows[i]["dateIn"].ToString();
                    int year = int.Parse(dateString.Substring(0, 4));
                    int month = int.Parse(dateString.Substring(5, 2));
                    int day = int.Parse(dateString.Substring(8, 2));
                    dateIn = new DateTime(year, month, day);
                    break;
                }

            }
            return dateIn;
        }
        private DateTime GetDateOut()
        {
            DateTime dateIn = DateTime.Now;
            for (int i = 0; i < fullDatabase1.BookingSummary.Rows.Count; i++)
            {
                if (fullDatabase1.Tables["BookingSummary"].Rows[i]["summaryID"].ToString() == currentUser.getSummaryID() + "")
                {
                    string dateString = fullDatabase1.Tables["BookingSummary"].Rows[i]["dateOut"].ToString();
                    int year = int.Parse(dateString.Substring(0, 4));
                    int month = int.Parse(dateString.Substring(5, 2));
                    int day = int.Parse(dateString.Substring(8, 2));
                    dateIn = new DateTime(year, month, day);
                    break;
                }

            }
            return dateIn;
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void resetColor(TextBox textBox)
        {
            textBox.BackColor = Color.White;
        }
    }

}